using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IStudentService
    {
        Task<List<UserResponse>> getAllByClass(string codeClass);
        Task UpdateStudent(Guid id);
        Task<ApiResponse<ClassOfStudentResponse>> GetAllClassByStudentId(string studentId);
        Task<ApiResponse<List<UserResponse>>> GetStudentsNotPassedClass(string year, int grade);
    }
}
