using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface ITeacherService
    {
        Task<ApiResponse<UserResponse>> getTeacherBySubjectAndClass(Guid subjectId, Guid classId);
        Task<ApiResponse<ClassResponse>> getStudentAndClassByAdvisor(Guid teacherId, string year);

        //Task<ApiResponse<ClassResponse>> getStudentAndClassBySubject(string teacherId, string year, string subjectId);

    }
}
