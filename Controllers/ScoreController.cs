using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService scoreService;
        public ScoreController(IScoreService scoreService)
        {
            this.scoreService = scoreService;
        }

        [HttpPost("scoreboard")]
        [Authorize]
        public async Task<IActionResult> InitializeScore(string studentId, string year, int semester)
        {
            var response = await scoreService.InitializeScoreBoard(studentId, year, semester);
            return Ok(response);
        }

        //Lấy điểm 1 môn của một hs
        [HttpGet("subject/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetScoreOfSubject(string studentId, [FromQuery] string year, [FromQuery] int semester, [FromQuery] Guid subjectId)
        {
            var response = await scoreService.GetScoreOfSubject(studentId, year, semester, subjectId);
            return Ok(response);
        }

        [HttpGet("semester/{semester}")]
        [Authorize]
        public async Task<IActionResult> GetScoreInSemester([FromQuery] string studentId, [FromQuery]string year, int semester)
        {
            var response = await scoreService.GetAllScoreInSemester(studentId, year, semester);
            return Ok(response);
        }


        //Lấy điểm 1 môn của nhiều học sinh thuộc một lớp mà giáo viên giảng dạy.
        [HttpPost("teacher/subject/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> GetScoreBySubject([FromQuery] string year, [FromQuery]int semester, [FromRoute]Guid subjectId, [FromBody] IEnumerable<GetAllScoreBySubjectRequest> studentIds)
        {
            var response = await scoreService.GetAllScoreBySubject(subjectId, year, semester, studentIds);
            return Ok(response);
        }


        //Cập nhật điểm số cho 1 môn học cho một học sinh tại một năm học và học kỳ xác định
        [HttpPut("students/{studentId}/scores/{year}/{semester}/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> UpdateScoreOfSubject(string studentId, string year, int semester, Guid subjectId, UpdateScoreOfSubjectRequest request)
        {
            var response = await scoreService.UpdateScoreOfSubject(studentId, year, semester, subjectId, request);
            return Ok(response);
        }

        //cập nhật điểm số 1 cột - 1 môn - nhiều học sinh của 1 lớp
        [HttpPut("subject/{subjectId}")]
        [Authorize]
        public async Task<IActionResult> UpdateScoreOfSubjectByColumn(Guid subjectId, [FromQuery] string year, [FromQuery] int semester, [FromBody]IEnumerable<ScoreByColumnRequest> scores)
        {
            var response = await scoreService.UpdateSubjectScoreByColumn(subjectId, year, semester, scores);
            return Ok(response);

        }
    }
}
