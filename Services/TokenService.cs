using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NL_THUD.Domains.Contracts;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NL_THUD.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly SymmetricSecurityKey _SecurityKey;
        private readonly string? _validIssuer;
        private readonly string? _validAudience;
        private readonly string? _expires;
        private readonly UserManager<Person> _userManager;

        public TokenService(IConfiguration configuration, UserManager<Person> userManager) {
        
            _userManager = userManager;
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();
            if (jwtSetting == null || string.IsNullOrEmpty(jwtSetting.SecretKey)) {
                throw new InvalidOperationException("Jwt secret key is not configured!");
            }
            _SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));//Dòng này được sử dụng để thiết lập bảo mật khi tạo JWT trong ASP.NET Core.
            _validAudience = jwtSetting.ValidAudience;
            _validIssuer = jwtSetting.ValidIssuer;
            _expires = jwtSetting.Expires;

        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            return refreshToken;
        }

        public async Task<string> GenerateToken(Person person)
        {
            var signingCredentials = new SigningCredentials(_SecurityKey, SecurityAlgorithms.HmacSha256); // ký
            var claims = await GetClaimsAsync(person); // lấy claim
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims); //tạo jwt token
            var tokenHandler = new JwtSecurityTokenHandler(); //Đối tượng giúp mã hóa JWT thành chuỗi string.
            var tokenString = tokenHandler.WriteToken(tokenOptions);
            return tokenString;
        }
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                    issuer: _validIssuer,
                    audience: _validAudience,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: signingCredentials,
                    claims: claims
                );
           
        }
        public async Task<List<Claim>> GetClaimsAsync(Person user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Gender",user.Gender.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Code", user.Code),
                new Claim("DoB", user.DoB.ToString()),
                new Claim("Avatar", user.Avatar ?? ""),
                new Claim(ClaimTypes.Role, user.ERole.ToString())
            };
            return claims;
        }
    }
}
