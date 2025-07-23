using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Response;
using NL_THUD.Mapping;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper, IStudentService studentService) {
            _userService = userService;
            _mapper = mapper;
            _studentService = studentService;
        }
        [HttpGet("filter")]
        public async Task<IActionResult> filter(string? codeClass, string? year, string? query, string role)
        {
            //if(year == null && role == "STUDENT")
            //{
                
            //} 
            var response = await _userService.filterUser(codeClass, year, query, role);
            var result = new ApiResponse<List<UserResponse>>
            {
                Code = "200",
                Message = "Success",
                Data = response
            };
            return Ok(result);
        }

        [HttpPut("block-user/{userId}")]
        public async Task<IActionResult> BlockAndUnblockUser(string userId)
        {
            var response = await _userService.BlockAndUnblockUser(userId);
            return Ok(response);
        }

        [HttpPut("set-active-user/{userId}")]
        public async Task<IActionResult> SetActiveUser(string userId)
        {
            var response = await _userService.SetActiveUser(userId);
            return Ok(response);
        }

        [HttpGet("student/not-pass-class/{year}")]
        public async Task<IActionResult> GetStudentNotPassClass(string year, [FromQuery]int grade)
        {
            var response = await _studentService.GetStudentsNotPassedClass(year, grade);
            return Ok(response);
        }
    }
}
