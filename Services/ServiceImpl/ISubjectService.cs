using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface ISubjectService
    {
        Task<ApiResponse<SubjectResponse>> addSubjectAsync(SubjectRequest request);
        Task<ApiResponse<string>> removeSubjectAsync(Guid id);
        Task<ApiResponse<SubjectResponse>> updateSubjectAsync(SubjectRequest request);
        Task<ApiResponse<SubjectResponse>> getSubjectById(Guid id);
        Task<ApiResponse<List<SubjectResponse>>> getAll();
        Task<ApiResponse<SubjectResponse>> AssignTeachersToSubject(Guid subjectId, string teacherId, Guid classId, string year, string? semester);
        Task<ApiResponse<SubjectResponse>> UnassignTeachersFromSubject(Guid subjectId, string teacherId, Guid classId);
        Task<ApiResponse<SubjectResponse>> UpdateTeachersForSubject(Guid subjectId, string teacherId, Guid ClassId, string year);   
    }
}
