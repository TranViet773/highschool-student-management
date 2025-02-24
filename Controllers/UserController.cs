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
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet("filter")]
        public async Task<IActionResult> filter(string? codeClass, string? year, string? query, string role)
        {
            var response = await _userService.filterUser(codeClass, year, query, role);
            var result = new ApiResponse<List<UserResponse>>
            {
                Code = "200",
                Message = "Success",
                Data = response
            };
            return Ok(result);
        }
    }
}
