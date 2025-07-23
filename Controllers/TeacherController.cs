using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IUserService _userService;
        public TeacherController(ITeacherService teacherService, IUserService userService)
        {
            _teacherService = teacherService;
            _userService = userService;
        }

        [HttpGet("get-by-subject")]
        [Authorize]
        public async Task<IActionResult> getBySubjectAndClass(Guid subjectId, Guid classId)
        {
            var response = await _teacherService.getTeacherBySubjectAndClass(subjectId, classId);
            return Ok(response);
        }

        [HttpGet("get-class-and-student-by-advisor")]
        [Authorize]
        public async Task<IActionResult> getClassAndStudentByAdvisor(Guid teacherId, string year)
        {
            var response = await _teacherService.getStudentAndClassByAdvisor(teacherId, year);
            return Ok(response);
        }

        [HttpGet("advisor/{teacherId}/class/student")]
        [Authorize]
        public async Task<IActionResult> getListClassAndStudentByAdvisor(string teacherId)
        {
            var response = await _teacherService.getListStudentAndClassByAdvisor(teacherId);
            return Ok(response);
        }

        //Update infor student
        [HttpPut("update-student/{id}")]
        [Authorize]
        public async Task<IActionResult> updateInforStudent(UserUpdateRequest request, Guid id)
        {
            var response = await _userService.UpdateUserAsync(request, id);
            return Ok(response);
        }

        [HttpGet("advisor/{classId}")]
        [Authorize]
        public async Task<IActionResult> getAdvisorByStudent(string classId, [FromQuery]string semester, [FromQuery]string year)
        {
            var response = await _userService.getAdvisorByStudent(classId, year, semester);
            return Ok(response);
        }


        //Lấy danh sách chủ nhiệm các lớp trong một năm học, học kỳ.
        [HttpGet("advisors/{year}")]
        [Authorize]
        public async Task<IActionResult> getAdvisorsInYear(string year)
        {
            var response = await _teacherService.getAdvisorsInYear(year);
            return Ok(response);
        }

        //GetTimeLine - Advisor, Teaching.
        [HttpGet("teaching/timeline/{teacherId}")]
        //[Authorize]
        public async Task<IActionResult> getTimeLineTeaching(string teacherId)
        {
            var response = await _teacherService.getTimeLineTeaching(teacherId);
            return Ok(response);
        }
    }
}
