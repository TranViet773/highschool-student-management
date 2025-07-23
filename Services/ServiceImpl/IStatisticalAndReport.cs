using CloudinaryDotNet;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IStatisticalAndReport
    {
        Task<ApiResponse<StatisticalGenerateResponse>> StatisticalGenerateAsync(string year);
        Task<ApiResponse<StatisticalResultOfGradeResponse>> StatisticalResultOfGradeAsync(string year, int grade);// thoongs kee keets qua theo khoi trong 1 nam hoc
        Task<ApiResponse<List<StatisticalResultOfGradeResponse>>> StatisticalResultsOfGradeByYearsAsync(string year, int grade);// thoongs kee keets qua theo khoi trong nhiều năm

    }
}
