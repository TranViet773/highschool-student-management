using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NL_THUD.Data;
using NL_THUD.Domains.Contracts;
using System.Text;

namespace NL_THUD.Services
{
    public static partial class ApplicationService
    {
        public static void ConfigureCors(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddCors(options =>
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()// cho phép tất cả các nguồn truy cập vào tài nguyên máy chủ, nên giới hạn lại bằng URL của máy chủ fe
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                })
            );
        }

        public static void ConfigureIdentity(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8; // toois thieu 8 ky tu
                options.Password.RequireDigit = true; // chua it nhat 1 so
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }

        public static void ConfigureJwt(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();
            if (jwtSetting == null || string.IsNullOrEmpty(jwtSetting.SecretKey))
            {
                throw new InvalidCastException("Jwt secret key is not configured!");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)); // SymmetricSecurityKey ký token và xác thực token
            serviceDescriptors.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSetting.ValidIssuer,
                    ValidAudience = jwtSetting.ValidAudience,
                    IssuerSigningKey = secretKey
                };
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            message = "You are not authorized to access this resource. Please authenticate."
                        });
                        return context.Response.WriteAsync(result);
                    },
                };
            });
        }
    }
}
