using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IClassService _classService;
        public AuthController(IUserService userService, IClassService classService)
        {
            _userService = userService;
            _classService = classService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var response = await _userService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("import-excel")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportStudents([FromForm] IFormFile file, [FromForm] string role)
       {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Vui lòng chọn file Excel.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        // Bỏ qua dòng tiêu đề
                        if (reader.Depth == 0) continue;

                        DateOnly doB;
                        var value = reader.GetValue(6);

                        if (value is double d)
                        {
                            doB = DateOnly.FromDateTime(DateTime.FromOADate(d)); 
                        }
                        else if (value is DateTime dt)
                        {
                            doB = DateOnly.FromDateTime(dt);
                        }
                        else if (value is string str && DateOnly.TryParse(str, out doB))
                        {
                            // Nếu là chuỗi thì cố gắng parse thành ngày
                        }
                        else
                        {
                            return BadRequest($"Dữ liệu không hợp lệ ở cột 6: {value}");
                        }


                        
                       
                        string genderStr = reader.GetValue(4)?.ToString()?.Trim().ToLower();
                        bool gender = genderStr == "nam";

                        
                        var erole = ERole.STUDENT;
                        switch (role)
                        {
                            case "0": erole = ERole.STUDENT; break;
                            case "1": erole = ERole.PARENT; break;
                            case "2": erole = ERole.TEACHER; break;
                            case "4": erole = ERole.MANAGERMENT_STAFF; break;
                        }

                        UserRegisterRequest userRegisterRequest = new UserRegisterRequest
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3),
                            Gender = gender,
                            Phone = reader.GetString(5),
                            DoB = doB,
                            ERole = erole
                        };

                        var response = await _userService.RegisterAsync(userRegisterRequest);

                        if (!string.IsNullOrEmpty(reader.GetString(7))){
                            var Class = await _classService.GetClassByCode(reader.GetString(7));
                            if (Class.Classes_Id == null) {
                                throw new Exception("Class do not exist!");
                            }
                            else
                            {
                                var ClassStudent = await _classService.AddStudentToClass(response.Code, Class.Classes_Id);
                            }
                        }
                    }
                }
            }

            return Ok("Successfully");
        }


        [HttpGet("get-all/{role}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(string role)
        {
            var response = await _userService.GetAllUsersAsync(role);
            return Ok(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] UserLoginRequest request)
        {
            var response = await _userService.LogInAsync(request);
            return Ok(response);
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = await _userService.GetCurrentUserAsync();
            return Ok(response);
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return Ok(response);
        }

        [HttpDelete("delete-user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(Guid id)
        {
            var response = await _userService.DeleteUserById(id);
            return Ok(response);
        }

        //Refresh token
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _userService.RefreshToken(request);
            return Ok(response);
        }

        //Revoke refresh token// logout
        [HttpPost("revoke-refresh-token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _userService.RevokeRefreshToken(request);
            if (response != null && response.Message == "Refresh token revoked successfully!")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        //Changepassword
        [HttpPost("change-password/{id}")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePasswordRequest request, Guid id)
        {
            var response = await _userService.changePasswordAsync(request, id);
            return Ok(response);
        }

        //UpdateUser
    }
}
