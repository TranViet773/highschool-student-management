using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class ScoreService : IScoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScoreService> _logger;
        private readonly IMapper _mapper;
        public ScoreService(ApplicationDbContext applicationDbContext, ILogger<ScoreService> logger, IMapper mapper)
        {
            this._context = applicationDbContext;
            this._logger = logger;
            this._mapper = mapper;
        }

        //Lấy điểm của các học theo môn học: Giáo viên chấm điểm
        public async Task<ApiResponse<ScoreBoardResponse>> GetAllScoreBySubject( Guid subjectId, string year, int semester, IEnumerable<GetAllScoreBySubjectRequest> studentIdList)
        {
            var studentIds = studentIdList.Select(s => s.studentId).ToList();
            var response = new ScoreBoardResponse();
            var student_score = await _context.Student_Score
                .Where(s => studentIds.Contains(s.StudentId)
                         && s.SubjectId == subjectId
                         && s.Year == year
                         && s.Semester == semester)
                .ToListAsync();
            response.scoreOfSubjectResponses = _mapper.Map<List<ScoreOfSubjectResponse>>(student_score);
            return new ApiResponse<ScoreBoardResponse>
            {
                Code = "200",
                Message = "Lấy danh sách điểm thành công!",
                Data = response
            };
        }


        public async Task<ApiResponse<ScoreBoardResponse>> GetAllScoreInSemester(string studentId, string year, int semester)
        {
            var subjectScores = await _context.Student_Score.Where(s => s.StudentId == studentId && s.Year == year && s.Semester == semester).ToListAsync();

            double avg_semester = 0;
            int i = 0;
            var response = new ScoreBoardResponse();
            if (subjectScores != null)
            {
                foreach (var subjectScore in subjectScores)
                {
                    i++;
                    if (subjectScore.AverageScore == null)
                    {
                        avg_semester = 0;
                        i = 0;
                        break;
                    }
                    avg_semester += subjectScore.AverageScore.Value;
                }

                response.scoreOfSubjectResponses = _mapper.Map<List<ScoreOfSubjectResponse>>(subjectScores);
                response.Comment = "";// -------------------------- để sau
            }

            if(avg_semester == 0)
            {
                response.EConduct = Models.Enum.EConduct.NULL;
                response.EPerformance = Models.Enum.EPerformance.NULL;
                response.Grade = 0;
                response.AVG_Semester_Score = 0;
                response.Classification = EClassification.NULL;
                response.Comment = null;
            }
            else
            {
                response.AVG_Semester_Score = avg_semester/i;
                response.EConduct = Models.Enum.EConduct.GOOD; // ---------------- giả sử

                if (avg_semester >= 8.5)
                    response.EPerformance = Models.Enum.EPerformance.GOOD;
                else if (avg_semester >= 7.0)
                    response.EPerformance = Models.Enum.EPerformance.FAIR;
                else if (avg_semester >= 5.0)
                    response.EPerformance = Models.Enum.EPerformance.AVARAGE;
                else
                    response.EPerformance = Models.Enum.EPerformance.WEAK;

                //Classification - Xếp Loại
                if (response.EPerformance == EPerformance.GOOD && response.EConduct == EConduct.GOOD)
                {
                    response.Classification = EClassification.GOOD;
                }
                else if ((response.EPerformance == EPerformance.FAIR && response.EConduct >= EConduct.FAIR) ||
                         (response.EPerformance == EPerformance.GOOD && response.EConduct == EConduct.FAIR))
                {
                    response.Classification = EClassification.FAIR;
                }
                else if ((response.EPerformance == EPerformance.AVARAGE && response.EConduct >= EConduct.AVARAGE) ||
                         (response.EPerformance == EPerformance.FAIR && response.EConduct == EConduct.AVARAGE))
                {
                    response.Classification = EClassification.AVARAGE;
                }
                else
                {
                    response.Classification = EClassification.WEAK;
                }
            }

            return new ApiResponse<ScoreBoardResponse>
            {
                Code = "200",
                Message = "Getting score in semester is successful!",
                Data = response
            };
        }

        public async Task<ApiResponse<ScoreOfSubjectResponse>> GetScoreOfSubject(string studentId, string year, int semester, Guid subjecId)
        {
            var subjectScore = await _context.Student_Score.FirstOrDefaultAsync(s => s.StudentId == studentId
                                                                                && s.Year == year && s.Semester == semester && s.SubjectId == subjecId);
            if (subjectScore is null)
            {
                return new ApiResponse<ScoreOfSubjectResponse>
                {
                    Code = "404",
                    Message = "No subjectScore found!",
                    Data = null
                };
            }
            return new ApiResponse<ScoreOfSubjectResponse>
            {
                Code = "200",
                Message = "Getting subjectScore is successful!",
                Data = _mapper.Map<ScoreOfSubjectResponse>(subjectScore)
            };
        }

        public async Task<ApiResponse<string>> InitializeScoreBoard(string studentId, string year, int semester)
        {
            //check xem user có tồn tại bảng điểm chưa.
            var student_score = await _context.Student_Score.AnyAsync(s => s.Year == year && s.Semester == semester && s.StudentId == studentId);
            if (student_score)
            {
                return new ApiResponse<string>
                {
                    Code = "409",
                    Message = "Scoreboard has already been initialized.",
                    Data = "Scoreboard has already been initialized."
                };  
            }
            //Lấy danh sách các Id subject.
            var subjectIdList = await _context.Subjects.Select(s => new
            {
                subjectId = s.Subject_Id
            }).ToListAsync();

            //Mình sẽ lặp qua các danh sách này
            foreach (var subjectId in subjectIdList) {
                try
                {
                    var score = new Student_Score
                    {
                        StudentId = studentId,
                        Year = year,
                        Semester = semester,
                        SubjectId = subjectId.subjectId
                    };
                    await _context.Student_Score.AddAsync(score);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex) { 
                    _logger.LogError("Có lỗi xảy ra khi thêm mội student_score!" + ex.Message);
                    return new ApiResponse<string>
                    {
                        Code = "500",
                        Data = null,
                        Message = "Lỗi khi tạo ScoreBoard: " + ex.Message
                    };
                }
            }
            return new ApiResponse<string>
            {
                Code = "200",
                Data = "Successfully!",
                Message = "Successfully created a new ScoreBoard!"
            };

        }

        public async Task<ApiResponse<ScoreOfSubjectResponse>> UpdateScoreOfSubject(string studentId, string year, int semester, Guid subjecId, UpdateScoreOfSubjectRequest request)
        {
            var scoreBoard = await _context.Student_Score.FirstOrDefaultAsync(s => s.StudentId ==  studentId && s.Year == year && s.Semester == semester && s.SubjectId == subjecId);
            if (scoreBoard == null) {
                _logger.LogError("ScoreBoard was not found!");
                return new ApiResponse<ScoreOfSubjectResponse>
                {
                    Code = "404",
                    Message = "ScoreBoard was not found!",
                    Data = null
                };
            }
            try
            {
                _mapper.Map(request, scoreBoard);
                double? temp = request.OralScore;
                double? temp2 = scoreBoard.OralScore;
                if(request.OralScore != null) { 
                    scoreBoard.OralScore_Timestamp = DateTime.Now;
                }
                if (request.TestScore != null) scoreBoard.TestScore_Timestamp = DateTime.Now;
                if (request.QuizScore != null) scoreBoard.QuizScore_Timestamp = DateTime.Now;
                if (request.FinalExamScore != null) scoreBoard.FinalExamScore_Timestamp = DateTime.Now;
                _context.Student_Score.Update(scoreBoard);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ApiResponse<ScoreOfSubjectResponse>
                {
                    Code = "500",
                    Message = "An error occurred while saving!",
                    Data = null
                };
            }

            var response = _mapper.Map<ScoreOfSubjectResponse>(scoreBoard);
            response.OralScore_Expire = response.OralScore != null ? scoreBoard.OralScore_Timestamp?.AddDays(7) : null;
            response.QuizScore_Expire = response.QuizScore != null ? scoreBoard.QuizScore_Timestamp?.AddDays(7) : null;
            response.TestScore_Expire = response.TestScore != null ? scoreBoard.TestScore_Timestamp?.AddDays(7) : null;
            response.FinalExamScore_Expire = response.FinalExamScore != null ? scoreBoard.FinalExamScore_Timestamp?.AddDays(7) : null;
            return new ApiResponse<ScoreOfSubjectResponse>
            {
                Code = "200",
                Message = "Updating Student_Score was succssful!",
                Data = response
            };
        }

        public async Task<ApiResponse<ScoreOfSubjectResponse>> UpdateSubjectScoreByColumn(Guid subjectId, string year, int semester, IEnumerable<ScoreByColumnRequest> scores)
        {
            _logger.LogInformation("Update điểm!");
            foreach(var item in scores)
            {
                var student_score = await _context.Student_Score.FirstOrDefaultAsync(s => s.Year == year && s.Semester == semester && s.SubjectId == subjectId && s.StudentId == item.studentId);
                if(student_score is null)
                {
                    return new ApiResponse<ScoreOfSubjectResponse>
                    {
                        Code = "404",
                        Message = "Student_Score was not found!",
                        Data = null
                    };
                }
                try
                {
                    switch (item.column)
                    {
                        case "ORALSCORE":
                            {
                                student_score.OralScore = item.score;
                                student_score.OralScore_Timestamp = DateTime.Now;
                                student_score.OralScore_Expire = DateTime.Now.AddMinutes(1);
                                break;
                            }
                        case "QUIZSCORE":
                            {
                                student_score.QuizScore = item.score;
                                student_score.QuizScore_Timestamp = DateTime.Now;
                                student_score.QuizScore_Expire = DateTime.Now.AddDays(7);
                                break;
                            }
                        case "TESTSCORE":
                            {
                                student_score.TestScore = item.score;
                                student_score.TestScore_Timestamp = DateTime.Now;
                                student_score.TestScore_Expire = DateTime.Now.AddDays(7);
                                break;
                            }
                        case "MIDTERMSCORE":
                            {
                                student_score.MidTearmScore = item.score; // nũa thêm rồi chỉnh sao nhe ===================================> this is Flag
                                student_score.MidTermScore_Timestamp = DateTime.Now;
                                student_score.MidTermScore_Expire = DateTime.Now.AddDays(7);
                                break;
                            }
                        case "FINALEXAMSCORE":
                            {
                                student_score.FinalExamScore = item.score;
                                student_score.FinalExamScore_Timestamp = DateTime.Now;
                                student_score.FinalExamScore_Expire = DateTime.Now.AddDays(7);
                                break;
                            }
                        case "finalEvaluation":
                            {
                                student_score.Comment = item.comment;
                                student_score.Comment_Expire = DateTime.Now.AddDays(7);
                                student_score.Comment_UpdateAt = DateTime.Now;
                                break;
                            }
                    }
                    _context.Student_Score.Update(student_score);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("An error occurred while updating: " + e.Message.ToString());
                    return new ApiResponse<ScoreOfSubjectResponse>
                    {
                        Code = "500",
                        Message = "An error occurred while updating: " + e.Message.ToString(),
                        Data = null
                    };
                } 
            }

            return new ApiResponse<ScoreOfSubjectResponse>
            {
                Code = "204",
                Message = "Updating was successful!",
                Data = null
            };
        }
    }
}
