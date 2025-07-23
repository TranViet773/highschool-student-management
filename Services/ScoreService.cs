using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;
using System.Net.WebSockets;

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

        //Lấy điểm của các học sinh theo môn học: Giáo viên chấm điểm
        public async Task<ApiResponse<ScoreBoardResponse>> GetAllScoreBySubject(Guid subjectId, string year, int semester, IEnumerable<GetAllScoreBySubjectRequest> studentIdList)
        {
            var studentIds = studentIdList.Select(s => s.studentId).ToList();
            var response = new ScoreBoardResponse();
            var student_score = await _context.Student_Score
                .Where(s => studentIds.Contains(s.StudentId)
                         && s.SubjectId == subjectId
                         && s.Year == year)
                .ToListAsync();

            var scoreOfSubject = student_score.Where(s => s.Semester == semester).ToList();
            response.scoreOfSubjectResponses = _mapper.Map<List<ScoreOfSubjectResponse>>(scoreOfSubject);

            var finalSubjectAverages = student_score
                .Where(s => s.AverageScore != null)
                .GroupBy(s => s.StudentId)
                .Select(g =>
                {
                    var hk1 = g.FirstOrDefault(x => x.Semester == 1)?.AverageScore ?? 0;
                    var hk2 = g.FirstOrDefault(x => x.Semester == 2)?.AverageScore ?? 0;
                    var count = (g.Any(x => x.Semester == 1) ? 1 : 0) + (g.Any(x => x.Semester == 2) ? 2 : 0);
                    var weightedAvg = count > 0 ? Math.Round((hk1 * 1 + hk2 * 2) / count, 2) : 0;

                    return new FinalSubjectAverageResponse
                    {
                        StudentId = g.Key.ToString(),
                        SubjectId = subjectId.ToString(),
                        FinalSubjectAverageScore = weightedAvg
                    };
                })
                .ToList();


            response.finalSubjectAverageScore = finalSubjectAverages;

            return new ApiResponse<ScoreBoardResponse>
            {
                Code = "200",
                Message = "Lấy danh sách điểm thành công!",
                Data = response
            };


        }
        public double? CalculateAVGScore(double? OralScore, double? OralScore1, double? OralScore2, double? OralScore3, double? QuizScore, double? QuizScore1, double? QuizScore2, double? QuizScore3, double? TestScore, double? FinalExamScore)
        {
            var listScore = new List<double?> { OralScore, QuizScore, TestScore, FinalExamScore };

            if (listScore.All(score => score.HasValue))
            {
                double oralScoreAvg = new List<double?> { OralScore, OralScore1, OralScore2, OralScore3 }
                    .Where(score => score.HasValue)
                    .Select(score => score.Value)
                    .DefaultIfEmpty(0)
                    .Average();

                double quizScoreAvg = new List<double?> { QuizScore, QuizScore1, QuizScore2, QuizScore3 }
                    .Where(score => score.HasValue)
                    .Select(score => score.Value)
                    .DefaultIfEmpty(0)
                    .Average();

                return (oralScoreAvg + quizScoreAvg + TestScore * 2 + FinalExamScore * 3) / 7;
            }

            return null;
        }

        public async Task<ApiResponse<ScoreBoardResponse>> GetAllScoreInSemester(string studentId, string year, int semester)
        {
            var subjectScores = await _context.Student_Score
                .Where(s => s.StudentId == studentId && s.Year == year)
                .Include(s => s.Subjects)
                .ToListAsync();

            var subjectScore = subjectScores.Where(s => s.Semester == semester).ToList();

            var finalSubjectAverageScore = subjectScores
                 .Where(s => s.AverageScore != null)
                 .GroupBy(s => s.SubjectId)
                 .Select(g =>
                 {
                     var hk1 = g.FirstOrDefault(x => x.Semester == 1)?.AverageScore ?? 0;
                     var hk2 = g.FirstOrDefault(x => x.Semester == 2)?.AverageScore ?? 0;
                     var count = (g.Any(x => x.Semester == 1) ? 1 : 0) + (g.Any(x => x.Semester == 2) ? 2 : 0);
                     var weightedAvg = count > 0 ? Math.Round((hk1 * 1 + hk2 * 2) / count, 2) : 0;

                     return new FinalSubjectAverageResponse
                     {
                         SubjectId = g.Key.ToString(),
                         FinalSubjectAverageScore = weightedAvg
                     };
                 })
                 .ToList();


            double? avg_semester = 0;
            int i = 0;
            int numSubjectsAvgLessThan65 = 0;
            int numSubjectsAvgLessThan50 = 0;
            int numSubjectsAvgLessThan35 = 0;
            int numSubjectsAvgGreateThan90 = 0;
            var response = new ScoreBoardResponse();
            if (subjectScore != null)
            {
                foreach (var item in subjectScore)
                {

                    if (item.AverageScore == null)
                    {
                        avg_semester = 0;
                        i = 0;
                        break;
                    }

                    if (item.ScoringType == true)
                    {
                        continue; // Chỉ tính điểm cho môn có điểm số (điểm chữ)
                    }
                    else
                    {
                        avg_semester += item.AverageScore;
                        i++;
                        if (item.AverageScore < 6.5) numSubjectsAvgLessThan65 += 1;
                        if (item.AverageScore < 5) numSubjectsAvgLessThan50 += 1;
                        if (item.AverageScore < 3.5) numSubjectsAvgLessThan35 += 1;
                        if (item.AverageScore >= 9.0) numSubjectsAvgGreateThan90 += 1;
                    }

                }

                response.scoreOfSubjectResponses = _mapper.Map<List<ScoreOfSubjectResponse>>(subjectScore);
                response.Comment = "";// -------------------------- để sau
            }

            if (avg_semester == 0) // Chưa hoàn thành tất cả các cột điểm yêu cầu.
            {
                response.EConduct = Models.Enum.EConduct.NULL;
                response.EPerformance = Models.Enum.EPerformance.NULL;
                response.Grade = 0;
                response.AVG_Semester_Score = 0;
                response.AVG_Semester_Score_Final = 0;
                response.Classification = EClassification.NULL;
                response.EConduct = Models.Enum.EConduct.NULL; // ---------------- giả sử
                var evaluation = await _context.Evaluations.FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == year && e.semester == semester);
                if (evaluation != null)
                {
                    response.Comment = evaluation.Evaluation_Content;
                }
                else
                {
                    response.Comment = string.Empty;
                }
            }
            else
            {
                response.AVG_Semester_Score = avg_semester / i;
                if (semester == 2)
                {
                    var score_semester1 = await _context.Student_Score.FirstOrDefaultAsync(s => s.StudentId == studentId && s.Year == year && s.Semester == 1);
                    response.AVG_Semester_Score_Final = (double)(avg_semester * 2 + score_semester1.AverageScore) / 3;
                }

                var evaluation = await _context.Evaluations.FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == year && e.semester == semester);
                response.Comment = evaluation?.Evaluation_Content;

                response.EConduct = await GetConduct(semester, year, studentId);
                Console.WriteLine(response.EConduct);

                //Performance
                if (avg_semester >= 8.0 && numSubjectsAvgLessThan65 == 0)
                    response.EPerformance = Models.Enum.EPerformance.EXCELENT; // Xuất sắc
                else if (avg_semester >= 6.5 && numSubjectsAvgLessThan50 == 0)
                    response.EPerformance = Models.Enum.EPerformance.GOOD; // Khá
                else if (avg_semester >= 5.0 && numSubjectsAvgLessThan35 == 0)
                    response.EPerformance = Models.Enum.EPerformance.AVARAGE; // đạt
                else
                    response.EPerformance = Models.Enum.EPerformance.WEAK; // chưa đạt

                // Classification - Khen thưởng
                if (response.EPerformance == EPerformance.EXCELENT && response.EConduct >= EConduct.EXCELENT && numSubjectsAvgGreateThan90 >= 6)
                {
                    response.Classification = EClassification.EXCELENT;
                }
                else if (response.EPerformance == EPerformance.EXCELENT && response.EConduct >= EConduct.EXCELENT)
                    response.Classification = EClassification.GOOD;

            }

            response.year = year;
            response.semester = semester;
            response.finalSubjectAverageScore = finalSubjectAverageScore;
            return new ApiResponse<ScoreBoardResponse>
            {
                Code = "200",
                Message = "Getting score in semester is successful!",
                Data = response
            };
        }

        public async Task<ApiResponse<List<ScoreBoardResponse>>> GetAllScoreToPrint(string studentId, int sesmester_start, int sesmester_end, string year_start, string year_end)
        {
            List<(string Year, int Semester)> years = GenerateYears(year_start, year_end, sesmester_start, sesmester_end);
            List<ScoreBoardResponse> scoreBoardResponses = new List<ScoreBoardResponse>();
            foreach (var e in years) {
                var responseScoreBoard = await GetAllScoreInSemester(studentId, e.Year, e.Semester);
                if (responseScoreBoard.Code == "200") {
                    scoreBoardResponses.Add(responseScoreBoard.Data);
                }
            }

            return new ApiResponse<List<ScoreBoardResponse>>
            {
                Code = "200",
                Message = "Getting All Score to print was successful!",
                Data = scoreBoardResponses
            };

        }

        public static List<(string Year, int Semester)> GenerateYears(string start, string end, int semesterStart, int semesterEnd)
        {
            List<(string, int)> years = new List<(string, int)>();
            int currentYear = int.Parse(start.Substring(0, 4));
            int endYear = int.Parse(end.Substring(0, 4));
            int currentSemester = semesterStart;

            while (currentYear < endYear || (currentYear == endYear) && currentSemester <= semesterEnd)
            {
                years.Add(($"{currentYear}-{currentYear + 1}", currentSemester));

                // Chuyển sang học kỳ tiếp theo
                if (currentSemester == 1)
                {
                    currentSemester = 2;
                }
                else
                {
                    currentSemester = 1;
                    currentYear++; // Nếu đã đến học kỳ 2, tăng năm lên
                }

                // Thoát khi đã vượt quá năm kết thúc
                if (currentYear == endYear && currentSemester > semesterEnd)
                {
                    break;
                }
            }

            return years;
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
            var subjectList = await _context.Subjects.Select(s => new
            {
                subjectId = s.Subject_Id,
                subjectScoringType = s.Subject_ScoringType
            }).ToListAsync();

            //Mình sẽ lặp qua các danh sách này
            foreach (var subject in subjectList) {
                try
                {
                    var score = new Student_Score
                    {
                        StudentId = studentId,
                        Year = year,
                        Semester = semester,
                        SubjectId = subject.subjectId,
                        ScoringType = subject.subjectScoringType
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

        public async Task<ApiResponse<String>> InitializeScoreBoardForSubject(string studentId, string year, int semester, string subjectId)
        {
            var student_score = await _context.Student_Score.AnyAsync(s => s.Year == year && s.Semester == semester && s.StudentId == studentId && s.SubjectId.ToString() == subjectId);
            if (student_score)
            {
                return new ApiResponse<string>
                {
                    Code = "409",
                    Message = "Scoreboard has already been initialized.",
                    Data = "Scoreboard has already been initialized."
                };
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Id.ToString() == subjectId);
            if (subject == null)
            {
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "Subject was not found!",
                    Data = "Subject was not found!"
                };
            }

            var newScore = new Student_Score
            {
                Semester = semester,
                Year = year,
                SubjectId = subject.Subject_Id,
                StudentId = studentId,
                ScoringType = subject.Subject_ScoringType
            };
            try
            {
                await _context.Student_Score.AddAsync(newScore);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    Code = "200",
                    Data = null,
                    Message = $"Tạo bảng điểm thành công cho môn học {subject.Subject_Name}"
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Có lỗi xảy ra khi thêm mội student_score!" + e.Message);
                return new ApiResponse<string>
                {
                    Code = "500",
                    Data = null,
                    Message = "Lỗi khi tạo ScoreBoard: " + e.Message
                };
            }
        }

        public async Task<ApiResponse<ScoreOfSubjectResponse>> UpdateScoreOfSubject(string studentId, string year, int semester, Guid subjecId, UpdateScoreOfSubjectRequest request)
        {
            var scoreBoard = await _context.Student_Score.FirstOrDefaultAsync(s => s.StudentId == studentId && s.Year == year && s.Semester == semester && s.SubjectId == subjecId);
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
                if (request.OralScore != null) {
                    scoreBoard.OralScore_Timestamp = DateTime.Now;
                }
                if (request.TestScore != null) scoreBoard.TestScore_Timestamp = DateTime.Now;
                if (request.QuizScore != null) scoreBoard.QuizScore_Timestamp = DateTime.Now;
                if (request.FinalExamScore != null) scoreBoard.FinalExamScore_Timestamp = DateTime.Now;
                _context.Student_Score.Update(scoreBoard);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
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
            //response.OralScore_Expire = response.OralScore != null ? scoreBoard.OralScore_Timestamp?.AddDays(7) : null;
            //response.QuizScore_Expire = response.QuizScore != null ? scoreBoard.QuizScore_Timestamp?.AddDays(7) : null;
            //response.TestScore_Expire = response.TestScore != null ? scoreBoard.TestScore_Timestamp?.AddDays(7) : null;
            //response.FinalExamScore_Expire = response.FinalExamScore != null ? scoreBoard.FinalExamScore_Timestamp?.AddDays(7) : null;
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
            foreach (var item in scores)
            {
                var student_score = await _context.Student_Score.FirstOrDefaultAsync(s => s.Year == year && s.Semester == semester && s.SubjectId == subjectId && s.StudentId == item.studentId);
                if (student_score is null)
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
                    var now = DateTime.Now;
                    var scoreProp = item.column;
                    var timestampProp = item.column + "_Timestamp";

                    var studentScoreType = typeof(Student_Score); //để lấy thông tin kiểu dữ liệu (type) của lớp Student_Score. Đây là một khái niệm trong lập trình phản chiếu (Reflection), cho phép bạn làm việc với thông tin về kiểu dữ liệu tại thời gian chạy (runtime).

                    // Gán giá trị điểm
                    var scoreProperty = studentScoreType.GetProperty(scoreProp); //tìm một thuộc tính (property) trong lớp `Student_Score` có tên là `scoreProp`.
                    if (scoreProperty != null)
                    {
                        scoreProperty.SetValue(student_score, item.score);
                    }

                    // Gán timestamp nếu tồn tại
                    var timestampProperty = studentScoreType.GetProperty(timestampProp);
                    if (timestampProperty != null)
                    {
                        timestampProperty.SetValue(student_score, now);
                    }

                    // Nếu là nhận xét cuối kỳ (comment)
                    if (item.column == "Comment")
                    {
                        student_score.Comment = item.comment;
                        student_score.Comment_Expire = now.AddDays(7);
                        student_score.Comment_UpdateAt = now;
                    }

                    student_score.AverageScore = CalculateAVGScore(student_score.OralScore, student_score.OralScore1, student_score.OralScore2,
                                                student_score.OralScore3, student_score.QuizScore, student_score.QuizScore1, student_score.QuizScore2,
                                                student_score.QuizScore3, student_score.TestScore, student_score.FinalExamScore);

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

        public async Task<ApiResponse<ResultFinalYear>> GetResultFinalYear(string studentId, string year)
        {
            var semester1 = await GetAllScoreInSemester(studentId, year, 1);
            var semester2 = await GetAllScoreInSemester(studentId, year, 2);

            var commentFinal = await _context.Evaluations.FirstOrDefaultAsync(s => s.StudentId == studentId && s.semester == 0);
            if (semester1 == null || semester2 == null)
            {
                return new ApiResponse<ResultFinalYear>
                {
                    Code = "404",
                    Message = "Chưa có điểm học kỳ 1 hoặc",
                    Data = null
                };
            }

            var response = new ResultFinalYear();
            //Get Conduct in the end of the year.
            response.EConduct_Final = CalculateFinalConduct(semester1.Data.EConduct, semester2.Data.EConduct);

            response.AVG_Semester_Score_Final = (semester1.Data.AVG_Semester_Score + semester2.Data.AVG_Semester_Score * 2) / 3;

            //Performance
            if (response.AVG_Semester_Score_Final >= 8.0)
                response.EPerformance_Final = EPerformance.EXCELENT; // Xuất sắc
            else if (response.AVG_Semester_Score_Final >= 6.5)
                response.EPerformance_Final = EPerformance.GOOD; // Khá
            else if (response.AVG_Semester_Score_Final >= 5.0)
                response.EPerformance_Final = EPerformance.AVARAGE; // Trung bình
            else
                response.EPerformance_Final = EPerformance.WEAK; // Yếu

            response.Classification_Final = semester2.Data.Classification;

            int numSubjectsAvgGreateThan90 = 0;
            foreach (var score1 in semester1.Data.finalSubjectAverageScore)
            {
                foreach (var score2 in semester2.Data.finalSubjectAverageScore)
                {
                    if ((score1.SubjectId == score2.SubjectId) && (score1.FinalSubjectAverageScore + score2.FinalSubjectAverageScore * 2) / 3 >= 9)
                    {
                        numSubjectsAvgGreateThan90 += 1;
                    }
                }
            }
            if (response.EPerformance_Final == EPerformance.EXCELENT && response.EConduct_Final >= EConduct.EXCELENT && numSubjectsAvgGreateThan90 >= 6)
            {
                response.Classification_Final = EClassification.EXCELENT;
            }
            else if (response.EPerformance_Final == EPerformance.EXCELENT && response.EConduct_Final >= EConduct.EXCELENT)
                response.Classification_Final = EClassification.GOOD;

            //Check có lên lớp hay không.
            var evalutation = await _context.Evaluations.FirstOrDefaultAsync(e => e.StudentId == studentId && e.year == year && e.semester == 0);
            response.Comment_Final = commentFinal?.Evaluation_Content;
            response.AVG_Semester1_Score = semester1.Data.AVG_Semester_Score;
            response.AVG_Semester2_Score = semester2.Data.AVG_Semester_Score;
            response.isPassClass = evalutation?.isPassClass;

            return new ApiResponse<ResultFinalYear>
            {
                Code = "200",
                Message = "",
                Data = response
            };
        }

        //Tính Hạnh Kiểm trong môt học kỳ
        private async Task<EConduct> GetConduct(int semester, string year, string userId)
        {
            var attendance = await _context.Attendances
                .Where(a => a.Attendance_Semester == semester
                         && a.Attendance_Year == year
                         && a.StudentsId == userId)
                .ToListAsync();

            int unexcused = attendance.Count(a => a.IsUnexcused);
            int totalAbsent = attendance.Count();

            if (unexcused <= 1 && totalAbsent <= 2)
                return EConduct.EXCELENT;

            if (unexcused <= 2 && totalAbsent <= 6)
                return EConduct.GOOD;

            if (unexcused <= 3 && totalAbsent <= 15)
                return EConduct.AVARAGE;

            return EConduct.WEAK;
        }

        //Tính hạnh kiểm cả năm
        public static EConduct CalculateFinalConduct(EConduct semester1, EConduct semester2)
        {
            // Chưa có đủ dữ liệu
            if (semester1 == EConduct.NULL || semester2 == EConduct.NULL)
                return EConduct.NULL;

            // Nếu học kỳ 2 là "Chưa đạt" hoặc học kỳ nào đó là "Chưa đạt"
            if (semester2 == EConduct.WEAK || semester1 == EConduct.WEAK)
                return EConduct.WEAK;

            // Nếu học kỳ 2 là "Tốt"
            if (semester2 == EConduct.EXCELENT)
                return EConduct.EXCELENT;

            // Nếu học kỳ 2 là "Khá"
            if (semester2 == EConduct.GOOD)
            {
                if (semester1 == EConduct.EXCELENT)
                    return EConduct.EXCELENT; // Có thể nâng lên
                return EConduct.GOOD;
            }

            // Nếu học kỳ 2 là "Đạt"
            if (semester2 == EConduct.AVARAGE)
                return EConduct.AVARAGE;

            // Trường hợp còn lại (dự phòng)
            return EConduct.AVARAGE;
        }


        //Thống kê điểm trung bình của một học sinh.
        public async Task<ApiResponse<StatisticalAvgSubjectResponse>> StatisticalAvgSubject(string studentId, string year)
        {
            try
            {
                var allScores = await _context.Student_Score
                    .Where(s => s.StudentId == studentId && s.Year == year)
                    .Include(s => s.Subjects)
                    .ToListAsync();

                var semester1 = allScores
                    .Where(s => s.Semester == 1)
                    .Select(s => new AvgSubjectResponse
                    {
                        SubjectName = s.Subjects.Subject_Name,
                        AverageScore = (CaculateAvgScoreForStatistical(s.OralScore, s.OralScore1, s.OralScore2, s.OralScore3,
                            s.QuizScore, s.QuizScore1, s.QuizScore2, s.QuizScore3, s.TestScore, s.FinalExamScore) ?? 0)
                    })
                    .ToList();

                var semester2 = allScores
                    .Where(s => s.Semester == 2)
                    .Select(s => new AvgSubjectResponse
                    {
                        SubjectName = s.Subjects.Subject_Name,
                        AverageScore = (CaculateAvgScoreForStatistical(s.OralScore, s.OralScore1, s.OralScore2, s.OralScore3,
                            s.QuizScore, s.QuizScore1, s.QuizScore2, s.QuizScore3, s.TestScore, s.FinalExamScore) ?? 0)
                    })
                    .ToList();

                var final = allScores
                    .GroupBy(s => s.Subjects.Subject_Name)
                    .Select(g =>
                    {
                        var semester1Score = g.Where(x => x.Semester == 1).Average(x => x.AverageScore ?? 0);
                        var semester2Score = g.Where(x => x.Semester == 2).Average(x => x.AverageScore ?? 0);
                        var finalAvg = 0.0;
                        if (semester1Score != 0 && semester2Score != 0)
                            finalAvg = (semester1Score + semester2Score) / 2;

                        return new AvgSubjectResponse
                        {
                            SubjectName = g.Key,
                            AverageScore = finalAvg
                        };
                    }).ToList();

                var response = new StatisticalAvgSubjectResponse
                {
                    AvgSubject_Semester1 = semester1,
                    AvgSubject_Semester2 = semester2,
                    AvgSubject_Final = final
                };

                return new ApiResponse<StatisticalAvgSubjectResponse>
                {
                    Code = "200",
                    Message = "Lấy điểm thành công!",
                    Data = response
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ApiResponse<StatisticalAvgSubjectResponse>
                {
                    Code = "500",
                    Message = "Lấy điểm không thành công!",
                    Data = null
                };
            }
        }

        public double? CaculateAvgScoreForStatistical(
            double? OralScore, double? OralScore1, double? OralScore2, double? OralScore3,
            double? QuizScore, double? QuizScore1, double? QuizScore2, double? QuizScore3,
            double? TestScore, double? FinalExamScore)
        {
            double totalScore = 0;
            double totalWeight = 0;

            // ✅ Trung bình miệng
            var oralScores = new List<double?> { OralScore, OralScore1, OralScore2, OralScore3 }
                .Where(s => s.HasValue)
                .Select(s => s.Value)
                .ToList();

            if (oralScores.Any())
            {
                double oralAvg = oralScores.Average();
                totalScore += oralAvg * 1;      // trọng số 1
                totalWeight += 1;
            }

            // ✅ Trung bình 15 phút
            var quizScores = new List<double?> { QuizScore, QuizScore1, QuizScore2, QuizScore3 }
                .Where(s => s.HasValue)
                .Select(s => s.Value)
                .ToList();

            if (quizScores.Any())
            {
                double quizAvg = quizScores.Average();
                totalScore += quizAvg * 1;      // trọng số 1
                totalWeight += 1;
            }

            // ✅ Kiểm tra hệ số 2 (Test)
            if (TestScore.HasValue)
            {
                totalScore += TestScore.Value * 2;
                totalWeight += 2;
            }

            // ✅ Kiểm tra hệ số 3 (Thi học kỳ)
            if (FinalExamScore.HasValue)
            {
                totalScore += FinalExamScore.Value * 3;
                totalWeight += 3;
            }

            // ✅ Trả về kết quả
            if (totalWeight > 0)
                return Math.Round(totalScore / totalWeight, 2);  // làm tròn 2 chữ số thập phân
            else
                return null;  // không có điểm nào
        }

        public Task<ApiResponse<string>> getYearEndSubjectAverage(string studentId, string year)
        {
            throw new NotImplementedException();
        }

        //Lấy danh sách bảng điểm của hs thuộc lớp để in ra.
        public async Task<ApiResponse<List<ScoreBoardOfClassInSemesterResponse>>> getScoreBoardStudent(string classId, int semester, string year)
        {
            try
            {
                var existingClass = await _context.Classes
                    .Include(c => c.Class_Student)
                    .FirstOrDefaultAsync(c => c.Classes_Id.ToString() == classId);
                if (existingClass == null)
                {
                    return new ApiResponse<List<ScoreBoardOfClassInSemesterResponse>>
                    {
                        Code = "404",
                        Message = "Không tìm thấy lớp",
                        Data = null
                    };
                }

                var result = new List<ScoreBoardOfClassInSemesterResponse>();
                var finalScoreData = new ResultFinalYear();
                foreach (var student in existingClass.Class_Student)
                {

                    //Lấy điểm
                    var scoreData = await GetAllScoreInSemester(student.Student_Id, year, semester);
                    if (semester == 2)
                    {
                        var data = await GetResultFinalYear(student.Student_Id, year);
                        finalScoreData = data.Data;
                    }
                    //Lấy thông tin học sinh
                    var studentData = await _context.Students.FirstOrDefaultAsync(s => s.Id == student.Student_Id);
                    var scoreBoardResponse = new ScoreBoardOfClassInSemesterResponse
                    {
                        StudentId = student.Student_Id,
                        StudentName = studentData?.Fullname,
                        DateOfBirth = studentData?.DoB,
                        StudentCode = studentData?.Code,
                        Semester = semester,
                        ScoresOfStudent = scoreData.Data,
                        resultFinalYear = finalScoreData
                    };

                    result.Add(scoreBoardResponse);
                }

                return new ApiResponse<List<ScoreBoardOfClassInSemesterResponse>>
                {
                    Code = "200",
                    Message = "Lấy danh sách bảng điểm của học sinh thành công!",
                    Data = result
                };

            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi trong quá trình lấy bảng điểm của cả lớp: ", e.Message);
                return new ApiResponse<List<ScoreBoardOfClassInSemesterResponse>>
                {
                    Code = "500",
                    Message = "Lỗi khi lấy danh sách bảng điểm của học sinh: " + e.Message,
                    Data = null
                };
            }
        }

        //Lấy kết quả cuối năm của một lớp để GVCN xem và thống kê
        public async Task<ApiResponse<FinalResultOfClassResponse>> GetFinalResultStudentsInClass(string classId, int semester, string year)
        {
            try
            {
                var existingClass = await _context.Classes
                    .Include(c => c.Class_Student)
                    .FirstOrDefaultAsync(c => c.Classes_Id.ToString() == classId);

                if (existingClass == null)
                {
                    return new ApiResponse<FinalResultOfClassResponse>
                    {
                        Code = "404",
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }
                //FinalResultOfClassResponse ==> [FinalClassSummaryDto, List<StudentFinalResultDto>]
                var result = new FinalResultOfClassResponse();
                var studentScoreData = new List<StudentFinalResultDto>();
                int quantityOfStudent = 0;
                if (semester != 0)
                {
                    foreach (var student in existingClass.Class_Student)
                    {
                        //Lấy điểm cuối năm
                        quantityOfStudent += 1;
                        var finalResult = await GetAllScoreInSemester(student.Student_Id, year, semester);
                        if (finalResult.Code == "200")
                        {
                            var studentData = await _context.Students.FirstOrDefaultAsync(s => s.Id == student.Student_Id);
                            var attendance = await _context.Attendances
                                .Where(a => a.Attendance_Semester == semester
                                         && a.Attendance_Year == year
                                         && a.StudentsId == student.Student_Id)
                                .ToListAsync();

                            var studentFinalResult = new StudentFinalResultDto
                            {
                                StudentId = studentData.Id,
                                StudentName = studentData.Fullname,
                                DateOfBirth = studentData.DoB,
                                StudentCode = studentData.Code,
                                AVG_Semester_Score = finalResult.Data.AVG_Semester_Score,
                                Conduct = finalResult.Data.EConduct.ToString(),
                                Performance = finalResult.Data.EPerformance.ToString(),
                                Classify = finalResult.Data.Classification.ToString(),
                                QuantityOfAbsent = attendance.Count(), // lấy từ bảng Attendance
                                QuantityOfAbsentWithoutPermission = attendance.Count(a => a.IsUnexcused),// lấy từ bảng Attendance
                                QuantityOfAbsentWithPermission = attendance.Count(a => a.IsExcused),// lấy từ bảng Attendance
                                Rank = finalResult.Data.Grade,
                                scoreBoard = finalResult.Data
                                //Bổ sung khen thưởng và lên lớp.
                            };
                            studentScoreData.Add(studentFinalResult);
                        }
                    }
                } else
                {
                    // Lấy kết quả cả năm của học sinh
                    foreach (var student in existingClass.Class_Student)
                    {
                        //Lấy điểm cuối năm
                        quantityOfStudent += 1;
                        var finalResult = await GetResultFinalYear(student.Student_Id, year);
                        if (finalResult.Code == "200")
                        {
                            var studentData = await _context.Students.FirstOrDefaultAsync(s => s.Id == student.Student_Id);
                            var attendance = await _context.Attendances
                                .Where(a => a.Attendance_Year == year
                                         && a.StudentsId == student.Student_Id)
                                .ToListAsync();

                            var studentFinalResult = new StudentFinalResultDto
                            {
                                StudentId = studentData.Id,
                                StudentName = studentData.Fullname,
                                DateOfBirth = studentData.DoB,
                                StudentCode = studentData.Code,
                                AVG_Semester_Score = finalResult.Data.AVG_Semester_Score_Final,
                                Conduct = finalResult.Data.EConduct_Final.ToString(),
                                Performance = finalResult.Data.EPerformance_Final.ToString(),
                                Classify = finalResult.Data.Classification_Final.ToString(),
                                QuantityOfAbsent = attendance.Count(), // lấy từ bảng Attendance
                                QuantityOfAbsentWithoutPermission = attendance.Count(a => a.IsUnexcused),// lấy từ bảng Attendance
                                QuantityOfAbsentWithPermission = attendance.Count(a => a.IsExcused),// lấy từ bảng Attendance
                                Rank = finalResult.Data.Grade_Final,
                                isPassClass = finalResult.Data.isPassClass
                                //Bổ sung khen thưởng và lên lớp.
                            };
                            studentScoreData.Add(studentFinalResult);
                        }

                    }
                }

                result.StudentData = studentScoreData;
                result.Semester = semester;
                result.Year = year;
                result.QuantityOfStudent = quantityOfStudent;
                var summaryConduct = studentScoreData
                    .GroupBy(s => s.Conduct)
                    .Select(g => new SummaryItem
                    {
                        Name = g.Key,
                        Count = g.Count()
                    }).ToList();

                var FinalClassSummaryDto = new FinalClassSummaryDto();
                FinalClassSummaryDto.Conduct = summaryConduct;

                var summaryPerformance = studentScoreData
                    .GroupBy(s => s.Performance)
                    .Select(g => new SummaryItem
                    {
                        Name = g.Key,
                        Count = g.Count()
                    }).ToList();
                FinalClassSummaryDto.Performance = summaryPerformance;

                var summaryClassify = studentScoreData
                   .GroupBy(s => s.Classify)
                   .Select(g => new SummaryItem
                   {
                       Name = g.Key,
                       Count = g.Count()
                   }).ToList();
                FinalClassSummaryDto.Classify = summaryClassify;

                result.FinalResult = FinalClassSummaryDto;

                return new ApiResponse<FinalResultOfClassResponse>
                {
                    Code = "200",
                    Message = "Lấy điểm thành công!",
                    Data = result
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse<FinalResultOfClassResponse>
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình lấy dữ liệu cuối!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<SubjectScoreRatioOfClassResponse>> GetScoreRatioOfClass(string classId, int semester, string year, Guid subjectId)
        {
            try
            {
                //Lấy danh sách học sinh từ ClassId
                var existingClass = await _context.Classes
                    .Include(c => c.Class_Student)
                    .FirstOrDefaultAsync(c => c.Classes_Id.ToString() == classId);
                if (existingClass == null)
                {
                    return new ApiResponse<SubjectScoreRatioOfClassResponse>
                    {
                        Code = "404",
                        Message = "Không tìm thấy lớp học!",
                        Data = null
                    };
                }

                var studentIds = existingClass.Class_Student.Select(s => s.Student_Id).ToList();
                if (studentIds.Count == 0)
                {
                    return new ApiResponse<SubjectScoreRatioOfClassResponse>
                    {
                        Code = "404",
                        Message = "Không có học sinh trong lớp này!",
                        Data = null
                    };
                }


                //Lấy điểm của học sinh trong lớp theo năm và học kỳ
                var scoreSubjectOfStudent = await _context.Student_Score.Where(s => s.Year == year
                                        && s.Semester == semester
                                        && s.SubjectId == subjectId
                                        && studentIds.Contains(s.StudentId)
                                       ).ToListAsync();
                Console.WriteLine($"Điểm của học sinh trong lớp: {scoreSubjectOfStudent}");
                // group điểm để ra kết quả
                var response = new SubjectScoreRatioOfClassResponse
                {
                    RatioOfOralScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.OralScore).ToList()),
                    RatioOfOralScore1 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.OralScore1).ToList()),
                    RatioOfOralScore2 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.OralScore2).ToList()),
                    RatioOfOralScore3 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.OralScore3).ToList()),

                    RatioOfQuizScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.QuizScore).ToList()),
                    RatioOfQuizScore1 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.QuizScore1).ToList()),
                    RatioOfQuizScore2 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.QuizScore2).ToList()),
                    RatioOfQuizScore3 = CalculateRatio(scoreSubjectOfStudent.Select(s => s.QuizScore3).ToList()),

                    RatioOfMidTermScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.TestScore).ToList()),
                    RatioOfFinalScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.FinalExamScore).ToList()),
                    RatioOfAverageScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.AverageScore).ToList()),
                    RatioOfFinalAverageScore = CalculateRatio(scoreSubjectOfStudent.Select(s => s.AverageScore).ToList())
                };

                return new ApiResponse<SubjectScoreRatioOfClassResponse>
                {
                    Code = "200",
                    Message = "Lấy tỷ lệ điểm của lớp thành công!",
                    Data = response
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse<SubjectScoreRatioOfClassResponse>
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình lấy dữ liệu cuối!",
                    Data = null
                };
            }
        }

        private RatioOfScoreType CalculateRatio(List<double?> scoresRaw)
        {
            var scores = scoresRaw.Where(s => s.HasValue).Select(s => s.Value).ToList();
            int total = scores.Count;
            if (total == 0) return new RatioOfScoreType();

            int Count(Func<double, bool> condition) => scores.Count(condition);
            double ToPercent(int count) => Math.Round(count * 100.0 / total, 2);

            return new RatioOfScoreType
            {
                Ratio80to100 = new RatioScore { Quantity = Count(s => s >= 8.0), Ratio = ToPercent(Count(s => s >= 8.0)) },
                Ratio65to79 = new RatioScore { Quantity = Count(s => s >= 6.5 && s < 8.0), Ratio = ToPercent(Count(s => s >= 6.5 && s < 8.0)) },
                Ratio50to64 = new RatioScore { Quantity = Count(s => s >= 5.0 && s < 6.5), Ratio = ToPercent(Count(s => s >= 5.0 && s < 6.5)) },
                Ratio35to49 = new RatioScore { Quantity = Count(s => s >= 3.5 && s < 5.0), Ratio = ToPercent(Count(s => s >= 3.5 && s < 5.0)) },
                Ratio0to34 = new RatioScore { Quantity = Count(s => s >= 0.0 && s < 3.5), Ratio = ToPercent(Count(s => s >= 0.0 && s < 3.5)) },
                RatioMoreThan50 = new RatioScore { Quantity = Count(s => s >= 5.0), Ratio = ToPercent(Count(s => s >= 5.0)) }
            };
        }

    } 
}
