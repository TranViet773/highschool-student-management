using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models.Enum;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IReevaluationService
    {
        Task<ApiResponse<ReevaluationResponse>> CreateReevaluationAsync(ReevaluationRequest request);
        Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationAsync(Guid ReevaluationId, ReevaluationRequest request);
        Task<ApiResponse<string>> DeleteReevaluationAsync(Guid reevaluationId);
        Task<ApiResponse<ReevaluationResponse>> GetReevaluationByIdAsync(Guid reevaluationId);
        Task<ApiResponse<List<ReevaluationResponse>>> GetReevaluationListAsync();

        //Lấy danh sách thông qua status cho Admin quản lý duyệt.
        Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStatus(EStatusSubmittion eStatus);
        Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStatusAndYear(EStatusSubmittion eStatus, string year, string? query = null);
        Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStudent(Guid studentId, EStatusSubmittion eStatus);
        Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByTeacher(Guid teacherId, EStatusSubmittion eStatus, string year);

        //Duyệt đơn phúc khảo, chỉ Admin mới có thể thực hiện.
        Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationStatus(EStatusSubmittion eStatus, Guid ReevaluationId);

        //Cập nhật điểm lại cho học sinh nếu đơn phúc khảo được xét duyệt.
        Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationStatusAndUpdateScore(string year, int semester, double score, Guid ReevaluationId);
        ///Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByTeacher(Guid studentId, EStatusSubmittion eStatus);

        Task<ApiResponse<ReevaluationResponse>> BlockAndUnblockReevaluation(EStatusSubmittion eStatus, Guid ReevaluationId);

        //Lấy đơn phúc khảo theo danh sách các năm của học sinh
        Task<ApiResponse<List<ReevaluationsOfStudentResponse>>> GetReevaluationsOfStudent(string studentId, List<string> listYear);

    }
}
