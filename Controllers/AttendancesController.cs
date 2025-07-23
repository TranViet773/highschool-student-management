using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAttendance(AttendanceRequest request)
        {
            var response = await _attendanceService.CreateAttendance(request);
            return Ok(response);
        }

        [HttpPost("list")]
        public async Task<IActionResult> CreateListAttendance(ListAttendanceRequest request)
        {
            var response = await _attendanceService.CreateListAttendance(request);
            return Ok(response);
        }


        //Lấy danh sách điểm danh của một lớp
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetAllAttendancesByClassId([FromQuery] string year, Guid classId)
        {
            var response = await _attendanceService.GetAllAttendanceByClass(year, classId);
            return Ok(response);
        }

        //Lấy danh sách điểm danh của một lớp trong 1 ngày
        [HttpGet("class/{classId}/date")]
        public async Task<IActionResult> getAllAttendancesByDate([FromQuery] string date, [FromQuery] string year, Guid classId)
        {
            var response = await _attendanceService.GetAllAttendancesByDate(date, year, classId);
            return Ok(response);
        }

        //Lấy danh sách điểm danh của một lớp trong 1 tuần (đầu vào là 1 ngày thuộc tuần
        [HttpGet("class/{classId}/week")]
        public async Task<IActionResult> getAllAttendancesByWeek([FromQuery] string date, [FromQuery] string year, Guid classId)
        {
            var response = await _attendanceService.GetAllAttendancesByWeek(date, year, classId);
            return Ok(response);
        }

        [HttpDelete("{attendanceId}")]
        public async Task<IActionResult> deleteAttendanceById(string attendanceId)
        {
            var response = await _attendanceService.DeleteAttendanceById(attendanceId);
            return Ok(response);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> getAllAttendanceByStudentId(string studentId, [FromQuery]string year)
        {
            var response = await _attendanceService.GetAllAttendanceByStudentId(studentId, year);
            return Ok(response);
        }

        [HttpGet("statistics/class/{classId}")]
        public async Task<IActionResult> getAllAttendanceByStudentId(Guid classId, [FromQuery] string year)
        {
            var response = await _attendanceService.AbsenceStatisticsForTheMonth(classId, year);
            return Ok(response);
        }

        [HttpGet("all-year/student/{studentId}")]
        public async Task<IActionResult> GetAllAbsenceOfStudent([FromQuery] List<string> listYear, string studentId)
        {
            var response = await _attendanceService.GetAllAbsenceOfStudent(studentId, listYear);
            return Ok(response);
        }
    }
}
