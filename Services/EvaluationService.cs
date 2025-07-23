using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;   
        private readonly ILogger<EvaluationService> _logger;
        private readonly IScoreService _scoreService;  

        public EvaluationService(ApplicationDbContext context, IMapper mapper, ILogger<EvaluationService> _log, IScoreService scoreService)
        {
            _context = context ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
            _logger = _log ?? throw new ArgumentNullException();
            _scoreService = scoreService;   
        }

        public async Task<ApiResponse<EvaluationResponse>> CreateStudentEvaluation(string studentId,EvaluationRequest request) // semster = 0 => kết quả cả năm
        {
            try
            {
                var evaluation = await _context.Evaluations
                    .FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == request.year && e.semester == request.semester);

                if (evaluation == null)
                {
                    evaluation = _mapper.Map<Evaluation>(request);
                    evaluation.StudentId = studentId;
                    evaluation.UpdateAt = DateTime.Now;
                    await _context.Evaluations.AddAsync(evaluation);
                }
                else
                {
                    evaluation.Evaluation_Content = request.Evaluation_Content;
                    evaluation.UpdateAt = DateTime.Now;
                }

                // Lấy kết quả cuối năm để cập nhật Classification, Conduct, v.v.
                var finalResult = await _scoreService.GetResultFinalYear(studentId, request.year);
                if (finalResult.Code == "200" && finalResult.Data != null)
                {
                    evaluation.Classification = finalResult.Data.Classification_Final;
                    evaluation.Conduct = finalResult.Data.EConduct_Final;
                    evaluation.Performance = finalResult.Data.EPerformance_Final;
                    evaluation.Rank = finalResult.Data.Grade_Final;

                    // Tổng số ngày nghỉ
                    var totalAbsentDays = await _context.Attendances
                        .CountAsync(c => c.StudentsId == studentId && c.Attendance_Year == request.year);

                    if (totalAbsentDays <= 45 &&
                        finalResult.Data.EConduct_Final >= Models.Enum.EConduct.AVARAGE &&
                        finalResult.Data.EPerformance_Final >= Models.Enum.EPerformance.AVARAGE)
                    {
                        evaluation.isPassClass = true;
                    }
                    else
                    {
                        evaluation.isPassClass = false;
                    }
                }

                await _context.SaveChangesAsync();

                return new ApiResponse<EvaluationResponse>
                {
                    Code = evaluation.Evaluation_Id == Guid.Empty ? "201" : "200",
                    Message = evaluation.Evaluation_Id == Guid.Empty ? "Evaluation created successfully!" : "Evaluation updated successfully!",
                    Data = _mapper.Map<EvaluationResponse>(evaluation)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating/updating evaluation");
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<EvaluationResponse>> GetStudentEvaluation(string studentId, string year, int semester)
        {
            var studentEvaluation = await _context.Evaluations.FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == year && e.semester == semester);
            if (studentEvaluation == null) {
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "404",
                    Message = "Getting Evaluation of student was failed!",
                    Data = null
                };
            }
            return new ApiResponse<EvaluationResponse>
            {
                Code = "200",
                Message = "Getting Evaluation of student was successful!",
                Data = _mapper.Map<EvaluationResponse>(studentEvaluation)
            };
        }

        public async Task<ApiResponse<EvaluationResponse>> UpdateStudentEvaluation(string studentId, EvaluationRequest request)  // Hàm này vừa cập nhật thông tin Nhận xét cuối năm, mà còn cập nhật Kết quả khen thưởng cuối năm.
        {
            var evaluation = await _context.Evaluations.FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == request.year && e.semester == request.semester);
            if (evaluation == null)
            {
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "404",
                    Message = "Updating Evaluation of student was failed!",
                    Data = null
                };
            }

            try
            {
                evaluation.UpdateAt = DateTime.Now;
                //evaluation.UpdateTime_Expire = DateTime.Now;
                evaluation.Evaluation_Content = request.Evaluation_Content;

                //Thực hiện lấy kết quả cuối năm và cập nhật.

                var finalResult = await _scoreService.GetResultFinalYear(studentId, request.year);
                if(finalResult.Code == "200")
                {
                    evaluation.Classification = finalResult.Data.Classification_Final;
                    evaluation.Conduct = finalResult.Data.EConduct_Final;
                    evaluation.Performance = finalResult.Data.EPerformance_Final;
                    evaluation.Rank = finalResult.Data.Grade_Final;

                    //lấy số ngày nghỉ của hịc sinh trong một năm học
                    var totalAbsentDays = _context.Attendances.Count(c => c.StudentsId == studentId);
                    Console.WriteLine($"Total absent days: {totalAbsentDays}");
                    if (totalAbsentDays <= 45 && finalResult.Data.EConduct_Final >= Models.Enum.EConduct.AVARAGE && finalResult.Data.EPerformance_Final >= Models.Enum.EPerformance.AVARAGE)  // nếu số ngày nghỉ > 5 thì không được khen thưởng
                    {
                        evaluation.isPassClass = true; //đủ điều kiện lên lớp.
                    }
                    else
                    {
                        evaluation.isPassClass = false; // Không đủ điều kiện lên lớp.
                    }
                }

                _context.Update(evaluation);


                await _context.SaveChangesAsync();
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "200",
                    Message = "Evaluation updated successfully!",
                    Data = _mapper.Map<EvaluationResponse>(evaluation)
                };
            }
            catch (Exception ex) {
                _logger.LogError("Updating Evaluation of student was failed!");
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
