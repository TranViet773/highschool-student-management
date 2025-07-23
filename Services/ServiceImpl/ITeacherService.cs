using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface ITeacherService
    {
        Task<ApiResponse<UserResponse>> getTeacherBySubjectAndClass(Guid subjectId, Guid classId);
        Task<ApiResponse<ClassResponse>> getStudentAndClassByAdvisor(Guid teacherId, string year);

        //Task<ApiResponse<ClassResponse>> getStudentAndClassBySubject(string teacherId, string year, string subjectId);
        Task<ApiResponse<List<AdvisorsInYearResponse>>> getAdvisorsInYear(string year);


        //Lấy danh sách (lớp + học sinh) đã chủ nhiệm - log lịch sử chủ nhiệm.
        Task<ApiResponse<List<ClassResponse>>> getListStudentAndClassByAdvisor(string teacherId);

        Task<ApiResponse<List<TimeLineTeachingResponse>>> getTimeLineTeaching(string teacherId);
    }
}
