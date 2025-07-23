using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticalAndReportController : ControllerBase
    {
        private readonly IStatisticalAndReport statisticalAndReportService;
        public StatisticalAndReportController(IStatisticalAndReport statisticalAndReportService)
        {
            this.statisticalAndReportService = statisticalAndReportService;
        }


        [HttpGet("general")]
        [Authorize]
        public async Task<IActionResult> GetStatisticalGenerate([FromQuery]string year)
        {
            var response = await statisticalAndReportService.StatisticalGenerateAsync(year);
            return Ok(response);
        }

        [HttpGet("result-of-grade")]
        [Authorize]
        public async Task<IActionResult> GetStatisticalLearingPracticeRewardOfGradeInYear([FromQuery]string year, [FromQuery]int grade)
        {
            var response = await statisticalAndReportService.StatisticalResultOfGradeAsync(year, grade);
            return Ok(response);
        }

        [HttpGet("result-of-grade-by-years")]
        public async Task<IActionResult> StatisticalResultsOfGradeByYearsAsync([FromQuery] string year, [FromQuery] int grade)
        {
            var response = await statisticalAndReportService.StatisticalResultsOfGradeByYearsAsync(year, grade);
            return Ok(response);
        }
    }
}
