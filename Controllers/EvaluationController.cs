using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;
        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpGet("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetStudentEvaluation(string studentId, [FromQuery]string year, [FromQuery]int semester)
        {
            var response = await _evaluationService.GetStudentEvaluation(studentId, year, semester);
            return Ok(response);
        }

        [HttpPost("student/{studentId}")]
        [Authorize]
        public async Task<IActionResult> CreateStudentEvaluation(string studentId, EvaluationRequest request)
        {
            var response = await _evaluationService.CreateStudentEvaluation(studentId, request);
            return Ok(response);
        }

        [HttpPut("student/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateStudentEvaluation(string id, EvaluationRequest request)
        {
            var response = await _evaluationService.UpdateStudentEvaluation(id, request);
            return Ok(response);
        }
    }
}
