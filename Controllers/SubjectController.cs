using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> addSubject([FromBody] SubjectRequest subjectRequest)
        {
            var response = await _subjectService.addSubjectAsync(subjectRequest);
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteSubject(Guid id)
        {
            var response = await _subjectService.removeSubjectAsync(id);
            return Ok(response);
        }

        [HttpGet("get-by-id/{id}")]
        [Authorize]
        public async Task<IActionResult> getSubjectById(Guid id)
        {
            var response = await _subjectService.getSubjectById(id);
            return Ok(response);
        }

        [HttpGet("get-all")]
        [Authorize]
        public async Task<IActionResult> getAll()
        {
            var response = await _subjectService.getAll();
            return Ok(response);
        }

        [HttpPost("assign-teacher")]
        [Authorize]
        public async Task<IActionResult> assignTeacherForSubject(Guid subjectId, string teacherId, Guid classId, string year)
        {
            var semester = "all";
            var response = await _subjectService.AssignTeachersToSubject(subjectId, teacherId, classId, year, semester);
            return Ok(response);
        }

        [HttpPut("update-assigned-teacher")]
        [Authorize]
        public async Task<IActionResult> updatAssignedTeacher(Guid subjectId, string teacherId, Guid classId, string year)
        {
            var response = await _subjectService.UpdateTeachersForSubject(subjectId, teacherId, classId, year);
            return Ok(response);
        }

        [HttpDelete("delete-assigned-teacher")]
        [Authorize]
        public async Task<IActionResult> deleteAssignedTeacher(Guid subjectId, string teacherId, Guid classId)
        {
            var response = await _subjectService.UnassignTeachersFromSubject(subjectId, teacherId, classId);
            return Ok(response);
        }
    }
}
