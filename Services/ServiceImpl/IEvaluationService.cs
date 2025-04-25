using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IEvaluationService
    {
        Task<ApiResponse<EvaluationResponse>> GetStudentEvaluation(string studentId, string year, int semester);
        Task<ApiResponse<EvaluationResponse>> CreateStudentEvaluation(string studentId, EvaluationRequest request);
        Task<ApiResponse<EvaluationResponse>> UpdateStudentEvaluation(string studentId, EvaluationRequest request);
    }
}
