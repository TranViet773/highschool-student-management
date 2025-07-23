using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReevaluationController : ControllerBase
    {
        private readonly IReevaluationService _reevaluationService;

        public ReevaluationController(IReevaluationService reevaluationService)
        {
            _reevaluationService = reevaluationService;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> CreateReevaluation([FromBody]ReevaluationRequest request)
        {
            var response = await _reevaluationService.CreateReevaluationAsync(request);
            return Ok(response);
        }

        [HttpGet("status/{eStatus}")]
        [Authorize]
        public async Task<IActionResult> GetReevaluationByStatus(EStatusSubmittion eStatus)
        {
            var response = await _reevaluationService.GetAllReevaluationByStatus(eStatus);
            return Ok(response);
        }

        [HttpGet("status/{eStatus}/year")]
        [Authorize]
        public async Task<IActionResult> GetReevaluationByStatusAndYear(EStatusSubmittion eStatus, [FromQuery]string year, [FromQuery] string? query)
        {
            var response = await _reevaluationService.GetAllReevaluationByStatusAndYear(eStatus, year, query);
            return Ok(response);
        }

        [HttpPut("status/{ReevaluationId}")]
        [Authorize]
        public async Task<IActionResult> UpdateReevaluationStatus([FromBody] UpdateStatusReevaluationRequest request, [FromRoute]Guid ReevaluationId)
        {
            int status = int.Parse(request.EStatus);
            EStatusSubmittion eStatusSubmittion = (EStatusSubmittion)status;
            var response = await _reevaluationService.UpdateReevaluationStatus(eStatusSubmittion, ReevaluationId);
            return Ok(response);
        }

        [HttpGet("{ReevaluationId}")]
        [Authorize]
        public async Task<IActionResult> GetReevaluationById(Guid ReevaluationId)
        {
            var response = await _reevaluationService.GetReevaluationByIdAsync(ReevaluationId);
            return Ok(response);
        }

        [HttpGet("teacher/{teacherId}")]
        [Authorize]
        public async Task<IActionResult> GetReevaluationByTeacher(Guid teacherId, [FromQuery] EStatusSubmittion eStatus, [FromQuery]string year)
        {
            var response = await _reevaluationService.GetAllReevaluationByTeacher(teacherId, eStatus, year);
            return Ok(response);
        }

        [HttpPut("{reevaluationId}/score")]
        public async Task<IActionResult> UpdateReevaluationStatusAndUpdateScore([FromQuery]string year, [FromQuery]int semester, [FromBody]double score, [FromRoute]Guid reevaluationId)
        {
            var response = await _reevaluationService.UpdateReevaluationStatusAndUpdateScore(year, semester, score, reevaluationId);
            return Ok(response);
        }

        [HttpPut("{reevaluationId}")]
        public async Task<IActionResult> UpdateReevaluation([FromBody] ReevaluationRequest reevaluationRequest, [FromRoute] Guid reevaluationId)
        {
            var response = await _reevaluationService.UpdateReevaluationAsync(reevaluationId, reevaluationRequest);
            return Ok(response);
        }

        [HttpPut("{reevaluationId}/block")]
        public async Task<IActionResult> BlockAndUnblockReevaluation([FromBody] EStatusSubmittion eStatus, [FromRoute] Guid reevaluationId)
        {
            var response = await _reevaluationService.BlockAndUnblockReevaluation(eStatus , reevaluationId);
            return Ok(response);
        }

        [HttpGet("student/{studentId}/years")]
        public async Task<IActionResult> GetReevaluationsOfStudent(string studentId, [FromQuery] List<string> listYear)
        {
            var response = await _reevaluationService.GetReevaluationsOfStudent(studentId, listYear);
            return Ok(response);
        }
    }
}
