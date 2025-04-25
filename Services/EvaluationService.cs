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

        public EvaluationService(ApplicationDbContext context, IMapper mapper, ILogger<EvaluationService> _log)
        {
            _context = context ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
            _logger = _log ?? throw new ArgumentNullException();
        }

        public async Task<ApiResponse<EvaluationResponse>> CreateStudentEvaluation(string studentId,EvaluationRequest request)
        {
            try
            {
                Evaluation newEvaluation = new Evaluation();
                newEvaluation = _mapper.Map<Evaluation>(request);
                newEvaluation.UpdateAt = DateTime.Now;
                newEvaluation.UpdateTime_Expire = DateTime.Now;
                newEvaluation.StudentId = studentId;
                await _context.Evaluations.AddAsync(newEvaluation);
                await _context.SaveChangesAsync();
                return new ApiResponse<EvaluationResponse>
                {
                    Code = "201",
                    Message = "Creatting new evaluation was success!",
                    Data = _mapper.Map<EvaluationResponse>(newEvaluation)
                };
            }
            catch (Exception ex) {
                _logger.LogError("An Error occurred while create a new evaluation!");
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

        public async Task<ApiResponse<EvaluationResponse>> UpdateStudentEvaluation(string studentId,EvaluationRequest request)
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
                evaluation.UpdateTime_Expire = DateTime.Now;
                evaluation.Evaluation_Content = request.Evaluation_Content;
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
