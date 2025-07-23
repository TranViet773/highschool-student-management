using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IAttendanceService
    {

        //khi điểm danh, thực hiện tạo mới tại thời điểm hiện tại.
        Task<ApiResponse<AttendanceResponse>> CreateAttendance(AttendanceRequest request);
        Task<ApiResponse<AttendanceResponse>> CreateListAttendance(ListAttendanceRequest requests);
        Task<ApiResponse<AttendanceResponse>> UpdateAttendance(AttendanceRequest request);
        Task<ApiResponse<AttendanceResponse>> DeleteAttendance(AttendanceRequest request);
        Task<ApiResponse<AttendanceResponse>> GetAllAttendances(AttendanceRequest request);

        //Lấy danh sách theo lớp học, tại một ngày.
        Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendancesByDate(string date, string year, Guid classId);

        //Lấy danh sách theo lớp học, tại một năm học + một tuần, startDate+endDate.
        Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendancesByWeek(string date, string year, Guid classId);

        //Lấy danh sách theo lớp học, tại một năm học, nhất định.
        Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendanceByClass(string year, Guid classId); 
        Task<ApiResponse<AttendanceResponse>> DeleteAttendanceById(string id);

        Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendanceByStudentId(string studentId, string year);
        Task<ApiResponse<List<AbsenceStatisticsForTheMonthResponse>>> AbsenceStatisticsForTheMonth(Guid classId, string year);

        //Lấy tất cả các lượt vắng của học sinh dựa vào ListYear. Cho addmin xem thống kê.
        Task<ApiResponse<List<AbsentOfStudentResponse>>> GetAllAbsenceOfStudent(string studentId, List<string> listYear);
    } 
}
