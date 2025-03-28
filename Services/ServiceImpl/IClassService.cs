using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IClassService
    {
        Task<ClassResponse> AddClass(ClassRequest request);
        Task<ClassResponse> GetClassById(string id);
        Task DeleteClass(string id);
        Task<ClassResponse> GetClassByCode(string code);
        Task<ApiResponse<List<ClassResponse>>> GetClassByTeacher(string teacherId, string year, string semester); // lấy danh sách lớp mà giáo viên giảng dạy (môn học).
        //Task<ApiResponse<ClassResponse>> GetClassByTeacher
        Task<ApiResponse<List<ClassResponse>>> GetClassesByGrade(string grade, string year);
        Task<List<ClassResponse>> GetAllClasses(string year);
        Task<ApiResponse<String>> AddStudentToClass(string studentCode, Guid classId);
        Task<ApiResponse<String>> DeleteStudentToClass(string studentCode, Guid classCode);
        Task<ApiResponse<String>> ChangeStudentToClass(string studentCode, Guid classId);


    }
}
