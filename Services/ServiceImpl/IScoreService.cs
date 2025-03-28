using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IScoreService
    {
        //Khởi tạo bảng điểm tất cả các môn cho 1 học sinh.
        Task<ApiResponse<string>> InitializeScoreBoard(string studentId, string year, int semester);

        //lấy các cột điểm của một môn(Student_Score) của một học sinh.
        Task<ApiResponse<ScoreOfSubjectResponse>> GetScoreOfSubject(string studentId, string year, int semester, Guid subjecId);

        //lấy bảng điểm tất cả các môn trong một học kỳ.
        Task<ApiResponse<ScoreBoardResponse>> GetAllScoreInSemester(string studentId, string year, int semester);

        //lấy bảng điểm một môn của tất cả các học sinh đang học ở 1 lớp(teacherId), 1 năm học - học kỳ.
        Task<ApiResponse<ScoreBoardResponse>> GetAllScoreBySubject(Guid subjectId, string year, int semester, IEnumerable<GetAllScoreBySubjectRequest> studentIdlists);


        //Cập nhật điểm từng môn cho một học sinh.
        //(Có thể cập nhật 1 hoặc nhiều cột điểm)
        //Giảng Viên sẽ cập nhật điểm 1 môn cho mỗi học sinh
        Task<ApiResponse<ScoreOfSubjectResponse>> UpdateScoreOfSubject(string studentId, string year, int semester, Guid subjecId, UpdateScoreOfSubjectRequest request);

        //Cập nhật điểm từng môn, từng cột điểm cho tất cả học sinh thuộc một lớp(giảng dạy).  
        // Score = [{idstudent, score}]
        Task<ApiResponse<ScoreOfSubjectResponse>> UpdateSubjectScoreByColumn(Guid subjectId, string year, int semester, IEnumerable<ScoreByColumnRequest> scores); 

    }
}
