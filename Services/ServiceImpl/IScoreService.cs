using Microsoft.AspNetCore.Mvc;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IScoreService
    {
        //Khởi tạo bảng điểm tất cả các môn cho 1 học sinh.
        Task<ApiResponse<string>> InitializeScoreBoard(string studentId, string year, int semester);

        //Khởi tạo bảng điểm cho 1 môn cho 1 học sinh
        Task<ApiResponse<String>> InitializeScoreBoardForSubject(string studenId, string year, int semester, string subjectId);

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

        Task<ApiResponse<List<ScoreBoardResponse>>> GetAllScoreToPrint(string studentId, int sesmester_start, int sesmester_end, string year_start, string year_end);

        Task<ApiResponse<ResultFinalYear>> GetResultFinalYear(string studentId, string year);

        //Lấy dữ liệu điểm của một học sinh(HK1-HK2_Cả năm) để thống kê 
        Task<ApiResponse<StatisticalAvgSubjectResponse>> StatisticalAvgSubject(string studentId, string year);

        //Lấy điểm trung bình môn cuối năm của một học sinh.
        Task<ApiResponse<string>> getYearEndSubjectAverage(string studentId, string year); //Tên Môn, Mã Môn, Số điểm.


        //Lấy bảng điểm của tất cả học sinh (kèm thông tin học sinh) thuộc một trong một năm học và học kỳ
        Task<ApiResponse<List<ScoreBoardOfClassInSemesterResponse>>> getScoreBoardStudent(string classId, int semester, string year);

        //Lấy kết quả bao gồm hạnh kiểm - học lực - xếp loại - khen thưởng. theo năm học học kỳ của tất cả học sinh thuộc một lớp.
        Task<ApiResponse<FinalResultOfClassResponse>> GetFinalResultStudentsInClass(string classId, int semester, string year);

        // Lấy tỷ lệ điểm số của một lớp 
        Task<ApiResponse<SubjectScoreRatioOfClassResponse>> GetScoreRatioOfClass(string classId, int semester, string year, Guid subjectId);
    }
}
 