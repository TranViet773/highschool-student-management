using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models.Enum;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;
using NL_THUD.Data;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly IStudentService _studentService;
        private readonly ApplicationDbContext _context;
        public ClassController(IClassService classService, IStudentService studentService, ApplicationDbContext context) {
            _classService = classService;
            _studentService = studentService;
            _context = context;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateClass([FromBody] ClassRequest request)
        {
            var response = await _classService.AddClass(request);
            var result = new ApiResponse<ClassResponse>();
            if(response == null)
            {
                result.Code = "404";
                result.Data = response;
                result.Message = "Class is existed!";
            }
            else
            {
                result.Code = "200";
                result.Message = "Successfully!";
                result.Data = response;
            }
            
            return Ok(result);
        }
        [HttpGet("get-all-by-year")]
        [Authorize]
        public async Task<IActionResult> GetByYear(string year)
        {
            var response = await _classService.GetAllClasses(year);
            var result = new ApiResponse<List<ClassResponse>>
            {
                Code = "200",
                Message = "Successfully",
                Data = response
            };
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await _classService.DeleteClass(id);
            return Ok("Success");
        }

        [HttpGet("get-by-id/{id}")]
        [Authorize]
        public async Task<IActionResult> getById(string id)
        {
            var response = await _classService.GetClassById(id);


            var result = new ApiResponse<ClassResponse>
            {
                Code = "200",
                Message = "Success",
                Data = response
            };
            return Ok(result);
        }

        [HttpGet("get-by-grade")]
        [Authorize]
        public async Task<IActionResult> getByGrade(string grade, string year)
        {
            var response = await _classService.GetClassesByGrade(grade, year);
            return Ok(response);
        }

        [HttpGet("get-students-by-class")]
        [Authorize]
        public async Task<IActionResult> getStudentsByClass(string classCode)
        {
            var response = await _studentService.getAllByClass(classCode);
            var result = new ApiResponse<List<UserResponse>>
            {
                Code = "200",
                Message = "Success",
                Data = response
            };
            return Ok(result);
        }

        [HttpGet("teacher/{id}")]
        [Authorize]
        public async Task<IActionResult> getClassByTeacherId(string id, [FromQuery]string semester, [FromQuery]string year)
        {
            var response = await _classService.GetClassByTeacher(id, year, semester);
            return Ok(response);
        }

        [HttpPost("add-student")]
        [Authorize]
        public async Task<IActionResult> addStudent(string studentCode, Guid classId)
        {
            var response = await _classService.AddStudentToClass(studentCode, classId);
            return Ok(response);
        }

        [HttpDelete("delete-student")]
        [Authorize]
        public async Task<IActionResult> deleteStudent(string studentCode, Guid classId)
        {
            var response = await _classService.DeleteStudentToClass(studentCode, classId);
            return Ok(response);
        }

        [HttpPost("change-student")]
        [Authorize]
        public async Task<IActionResult> changeStudent(string studentCode, Guid classId)
        {
            var response = await _classService.ChangeStudentToClass(studentCode, classId);
            return Ok(response);
        }
    }
}
