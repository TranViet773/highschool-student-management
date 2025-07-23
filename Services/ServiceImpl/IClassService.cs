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
        Task<ApiResponse<List<ClassResponse>>> GetClassByTeacher(string teacherId, string? year, string? semester); // lấy danh sách lớp mà giáo viên giảng dạy (môn học).
        //Task<ApiResponse<ClassResponse>> GetClassByTeacher
        Task<ApiResponse<List<ClassResponse>>> GetClassesByGrade(string grade, string year);
        Task<List<ClassResponse>> GetAllClasses(string year);
        Task<ApiResponse<ClassResponse>> AddStudentToClass(string studentCode, Guid classId, string year);
        Task<ApiResponse<String>> DeleteStudentToClass(string studentCode, Guid classCode);
        Task<ApiResponse<String>> ChangeStudentToClass(string studentCode, Guid classId, string year);
        Task<ApiResponse<String>> ChangeStudentsToClass(List<string> studentCode, Guid classId, string year);

        Task<ApiResponse<String>> UpdateAdvisorForClass(string teacherId, Guid classId, string year, string semester);

        // Nâng 1 lớp
        Task<ApiResponse<String>> AdvanceStudentsToNextClass(Guid oldClassId, Guid newClassId, string year);
        //
        Task<ApiResponse<string>> CreateClassesToAdvance(string year, List<string> codeClasses);

        //Nâng học sinh
        Task<ApiResponse<string>> AdvanceClassForStudents(IEnumerable<string> studentCodes, Guid classId, string year);
    }
}
