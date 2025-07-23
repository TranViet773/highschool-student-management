using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NL_THUD.Data;
using NL_THUD.Dtos.Response;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class StatisticalAndReportService : IStatisticalAndReport
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoreService _scoreService;
        public StatisticalAndReportService(ApplicationDbContext context, IScoreService scoreService)
        {
            _context = context;
            _scoreService = scoreService;
        }

        public static List<string> GetAcademicYears(string inputYear, int n)
        {
            var result = new List<string>();

            // Parse the start year from the input
            string[] parts = inputYear.Split('-');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int endStartYear))
            {
                throw new ArgumentException("Invalid academic year format. Expected format: yyyy-yyyy");
            }

            // Generate list of academic years
            for (int i = n - 1; i >= 0; i--)
            {
                int startYear = endStartYear - i;
                result.Add($"{startYear}-{startYear + 1}");
            }

            return result;
        }

        public async Task<ApiResponse<StatisticalGenerateResponse>> StatisticalGenerateAsync(string year)
        {
            try
            {
                var NumberOfStudent = await _context.Students.CountAsync();
                var NumberOfTeacher = await _context.Teachers.CountAsync();
                var NumberOfStaff = await _context.ManagementStaffs.CountAsync();
                var NumberOfClass = await _context.Classes.Where(c => c.Class_Student.Any(cs => cs.Year == year)).CountAsync();
                var NumberOfSubject = await _context.Subjects.CountAsync();
                var NumberOfReevaluation = await _context.Reevaluations.CountAsync();
                var listYear = GetAcademicYears(year, 5);
                Console.WriteLine("listYear: ", listYear);

                var listNumberOfStudentByGrade = new List<NumberOfStudentByGrade>();
                var listNumberOfStudentByGender = new List<NumberOfStudentByGender>();
                var listNumberOfReevaluation = new List<NumberOfReevaluationByYear>();
                var listNumberOfStudentEnrolled = new List<NumberOfStudentEnrolled>();
                foreach (var item in listYear)
                {
                    var numberOfStudent10 = await _context.ClassStudents.Where(cs => cs.Class.Classes_Code.StartsWith("10") && cs.Year == item).CountAsync();
                    var numberOfStudent11 = await _context.ClassStudents.Where(cs => cs.Class.Classes_Code.StartsWith("11") && cs.Year == item).CountAsync();
                    var numberOfStudent12 = await _context.ClassStudents.Where(cs => cs.Class.Classes_Code.StartsWith("11") && cs.Year == item).CountAsync();
                    var numberOfStudentByGrade = new NumberOfStudentByGrade
                    {
                        Year = item,
                        NumberOfStudentOf10 = numberOfStudent10,
                        NumberOfStudentOf11 = numberOfStudent11,
                        NumberOfStudentOf12 = numberOfStudent12,
                    };
                    listNumberOfStudentByGrade.Add(numberOfStudentByGrade);

                    //Gender
                    var numberOfStudentMale = await _context.ClassStudents.Where(cs => cs.Students.Gender == true && cs.Year == item).CountAsync();
                    var numberOfStudentFemale = await _context.ClassStudents.Where(cs => cs.Students.Gender == false && cs.Year == item).CountAsync();

                    var numberOfStudentByGender = new NumberOfStudentByGender
                    {
                        Year = item,
                        Male = numberOfStudentMale,
                        Gender = numberOfStudentMale + numberOfStudentFemale,
                        Female = numberOfStudentFemale,
                    };
                    listNumberOfStudentByGender.Add(numberOfStudentByGender);

                    //Reevaluation //2 - 0,1,4

                    var reevaluationHandled = await _context.Reevaluations.Where(r => r.Reevaluation_Year == item && r.Reevaluation_Status == Models.Enum.EStatusSubmittion.COMPLETED).CountAsync();
                    var reevaluationNotHandledYet = await _context.Reevaluations.Where(r => r.Reevaluation_Year == item &&
                        (
                            r.Reevaluation_Status == Models.Enum.EStatusSubmittion.PENDING ||
                            r.Reevaluation_Status == Models.Enum.EStatusSubmittion.EDITTING ||
                            r.Reevaluation_Status == Models.Enum.EStatusSubmittion.SUBMITTED
                        )
                       ).CountAsync();

                    var numberOfReevaluationByYear = new NumberOfReevaluationByYear
                    {
                        Year = item,
                        NotHandleYet = reevaluationNotHandledYet,
                        Handled = reevaluationHandled
                    };
                    listNumberOfReevaluation.Add(numberOfReevaluationByYear);

                    //Enrolled
                    var yearSuffix = item.Substring(2, 2);
                    var enrolledStudent = await _context.Students
                        .Where(s => s.Code.Length >= 4 && s.Code.Substring(2, 2) == yearSuffix)
                        .CountAsync();
                    var enrolledStudents = new NumberOfStudentEnrolled
                    {
                        Year = item,
                        enrolledStudents = enrolledStudent
                    };
                    listNumberOfStudentEnrolled.Add(enrolledStudents);
                }
                return new ApiResponse<StatisticalGenerateResponse>
                {
                    Code = "200",
                    Message = "Lấy thông tin thống kê thành công!",
                    Data = new StatisticalGenerateResponse
                    {
                        NumberOfClass = NumberOfClass,
                        NumberOfStudent = NumberOfStudent,
                        NumberOfStaff = NumberOfStaff,
                        NumberOfTeacher = NumberOfTeacher,
                        NumberOfSubject = NumberOfSubject,
                        NumberOfReevaluation = NumberOfReevaluation,
                        NumberOfStudentEnrolleds = listNumberOfStudentEnrolled,
                        NumberOfReevaluationByYear = listNumberOfReevaluation,
                        NumberOfStudentByGender = listNumberOfStudentByGender,
                        NumberOfStudentByGrade = listNumberOfStudentByGrade,
                    }
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<StatisticalGenerateResponse>
                {
                    Code = "500",
                    Message = e.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<StatisticalResultOfGradeResponse>> StatisticalResultOfGradeAsync(string year, int grade)
        {
            var listStudents = await _context.Students
                .Include(s => s.ClassStudents)
                .Where(s => s.ClassStudents.Any(cs => cs.Year == year && cs.Class.Classes_Code.StartsWith(grade.ToString())))
                .ToListAsync();

            // Lưu kết quả từng học sinh
            var classLearningMap = new Dictionary<string, ResultLearningResponse>();
            var classPracticeMap = new Dictionary<string, ResultPracticeResponse>();
            var classRewardMap = new Dictionary<string, ResultRewardResponse>();
            var classNotPassMap = new Dictionary<string, ResultNotPassResponse>();

            foreach (var student in listStudents)
            {
                var studentClass = student.ClassStudents.FirstOrDefault(cs => cs.Year == year);
                var classByStudent = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == studentClass.Class_Id);
                var classCode = classByStudent.Classes_Code;
                if (classCode == null) continue;
                
                var result = await _scoreService.GetResultFinalYear(student.Id, year);
                var data = result?.Data;
                if (data == null) continue;

                // 1. Học lực
                if (!classLearningMap.ContainsKey(classCode))
                {
                    classLearningMap[classCode] = new ResultLearningResponse
                    {
                        ClassCode = classCode,
                        Excelent = 0,
                        Good = 0,
                        Passed = 0,
                        NotPassed = 0
                    };
                }

                switch (data.EPerformance_Final)
                {
                    case EPerformance.EXCELENT: classLearningMap[classCode].Excelent++; break;
                    case EPerformance.GOOD: classLearningMap[classCode].Good++; break;
                    case EPerformance.AVARAGE: classLearningMap[classCode].Passed++; break;
                    case EPerformance.WEAK: classLearningMap[classCode].NotPassed++; break;
                }

                // 2. Hạnh kiểm
                if (!classPracticeMap.ContainsKey(classCode))
                {
                    classPracticeMap[classCode] = new ResultPracticeResponse
                    {
                        ClassCode = classCode,
                        Excelent = 0,
                        Good = 0,
                        Passed = 0,
                        NotPassed = 0
                    };
                }

                switch (data.EConduct_Final)
                {
                    case EConduct.EXCELENT: classPracticeMap[classCode].Excelent++; break;
                    case EConduct.GOOD: classPracticeMap[classCode].Good++; break;
                    case EConduct.AVARAGE: classPracticeMap[classCode].Passed++; break;
                    case EConduct.WEAK: classPracticeMap[classCode].NotPassed++; break;
                }

                // 3. Khen thưởng
                if (!classRewardMap.ContainsKey(classCode))
                {
                    classRewardMap[classCode] = new ResultRewardResponse
                    {
                        ClassCode = classCode,
                        Excelent = 0,
                        Good = 0
                    };
                }

                if (data.Classification_Final == EClassification.EXCELENT)
                {
                    classRewardMap[classCode].Excelent++;
                }
                else if (data.Classification_Final == EClassification.GOOD)
                {
                    classRewardMap[classCode].Good++;
                }

                // 4. Không lên lớp
                if (!classNotPassMap.ContainsKey(classCode))
                {
                    classNotPassMap[classCode] = new ResultNotPassResponse
                    {
                        ClassCode = classCode,
                        NotPass = 0
                    };
                }

                if (data.isPassClass == false)
                {
                    classNotPassMap[classCode].NotPass++;
                }
            }

            return new ApiResponse<StatisticalResultOfGradeResponse>
            {
                Code = "200",
                Message = "Thống kê thành công",
                Data = new StatisticalResultOfGradeResponse
                {
                    Year = year,
                    Learnings = classLearningMap.Values.ToList(),
                    Practices = classPracticeMap.Values.ToList(),
                    Reward = classRewardMap.Values.ToList(),
                    NotPassedClass = classNotPassMap.Values.ToList()
                }
            };
        }

        public async Task<ApiResponse<List<StatisticalResultOfGradeResponse>>> StatisticalResultsOfGradeByYearsAsync(string year, int grade)
        {
            var listYear = GetAcademicYears(year, 5); // VD: ["2025-2026", "2024-2025", ..., "2021-2022"]
            var resultList = new List<StatisticalResultOfGradeResponse>();

            foreach (var y in listYear)
            {
                var students = await _context.Students
                    .Include(s => s.ClassStudents)
                    .Where(s => s.ClassStudents.Any(cs => cs.Year == y && cs.Class.Classes_Code.StartsWith(grade.ToString())))
                    .ToListAsync();

                var resultLearning = new ResultLearningResponse
                {
                    ClassCode = $"Khối {grade}",
                    Excelent = 0,
                    Good = 0,
                    Passed = 0,
                    NotPassed = 0
                };

                var resultPractice = new ResultPracticeResponse
                {
                    ClassCode = $"Khối {grade}",
                    Excelent = 0,
                    Good = 0,
                    Passed = 0,
                    NotPassed = 0
                };

                var resultReward = new ResultRewardResponse
                {
                    ClassCode = $"Khối {grade}",
                    Excelent = 0,
                    Good = 0
                };

                var resultNotPass = new ResultNotPassResponse
                {
                    ClassCode = $"Khối {grade}",
                    NotPass = 0
                };

                foreach (var student in students)
                {
                    var studentClass = student.ClassStudents.FirstOrDefault(cs => cs.Year == y);
                    if (studentClass == null) continue;

                    var result = await _scoreService.GetResultFinalYear(student.Id, y);
                    var data = result?.Data;
                    if (data == null) continue;

                    // Học lực
                    switch (data.EPerformance_Final)
                    {
                        case EPerformance.EXCELENT: resultLearning.Excelent++; break;
                        case EPerformance.GOOD: resultLearning.Good++; break;
                        case EPerformance.AVARAGE: resultLearning.Passed++; break;
                        case EPerformance.WEAK: resultLearning.NotPassed++; break;
                    }

                    // Hạnh kiểm
                    switch (data.EConduct_Final)
                    {
                        case EConduct.EXCELENT: resultPractice.Excelent++; break;
                        case EConduct.GOOD: resultPractice.Good++; break;
                        case EConduct.AVARAGE: resultPractice.Passed++; break;
                        case EConduct.WEAK: resultPractice.NotPassed++; break;
                    }

                    // Khen thưởng
                    if (data.Classification_Final == EClassification.EXCELENT)
                        resultReward.Excelent++;
                    else if (data.Classification_Final == EClassification.GOOD)
                        resultReward.Good++;

                    // Không lên lớp
                    if (data.isPassClass == false)
                        resultNotPass.NotPass++;
                }

                resultList.Add(new StatisticalResultOfGradeResponse
                {
                    Year = y,
                    Learnings = new List<ResultLearningResponse> { resultLearning },
                    Practices = new List<ResultPracticeResponse> { resultPractice },
                    Reward = new List<ResultRewardResponse> { resultReward },
                    NotPassedClass = new List<ResultNotPassResponse> { resultNotPass }
                });
            }

            return new ApiResponse<List<StatisticalResultOfGradeResponse>>
            {
                Code = "200",
                Message = "Thống kê thành công",
                Data = resultList
            };
        }


    }
}
