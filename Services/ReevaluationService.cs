using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;
using System.Net.NetworkInformation;

namespace NL_THUD.Services
{
    public class ReevaluationService : IReevaluationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReevaluationService> _logger;
        private readonly IMapper _mapper;
        private readonly IScoreService _scoreService;
        private readonly ICurrentUserService _currentUserService;
        public ReevaluationService(ApplicationDbContext context, IMapper mapper, ILogger<ReevaluationService> logger, IScoreService scoreService, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _scoreService = scoreService;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<ReevaluationResponse>> BlockAndUnblockReevaluation(EStatusSubmittion eStatus, Guid ReevaluationId)
        {
            try
            {
                var reevaluation = await _context.Reevaluations.FirstOrDefaultAsync(r => r.Reevaluation_Id == ReevaluationId);
                if (reevaluation == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Data = null,
                        Message = "Đơn phúc khảo không tồn tại!"
                    };
                }

                if(eStatus == EStatusSubmittion.BLOCKED && reevaluation.Reevaluation_Status == EStatusSubmittion.BLOCKED) // Đang block, unblock
                {
                    reevaluation.Reevaluation_Status = EStatusSubmittion.EDITTING;
                }
                else if(eStatus != EStatusSubmittion.BLOCKED && reevaluation.Reevaluation_Status != EStatusSubmittion.BLOCKED)
                {
                    reevaluation.Reevaluation_Status = EStatusSubmittion.BLOCKED;
                }
                await _context.SaveChangesAsync();

                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Cập nhật trạng thái thành công!",
                    Data = _mapper.Map<ReevaluationResponse>(reevaluation)
                };
            }
            catch(Exception ex)
            {
                _logger.LogError($"Có lỗi trong quá trình cập nhật: {ex.Message}");
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ReevaluationResponse>> CreateReevaluationAsync(ReevaluationRequest request)
        {
            _logger.LogInformation("Create a new Reevaluation!");
            try
            {
                //Check userId, teacherId, subjectId có tồn tại không
                var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Id.ToString() == request.SubjectsId);
                if (subject == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Message = "Mã môn học không tồn tại!",
                        Data = null
                    };
                }

                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == request.StudentsId);
                if (student == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "400",
                        Message = "Mã học sinh không tồn tại!",
                        Data = null
                    };
                }

                var data = _mapper.Map<Reevaluation>(request);
                _logger.LogInformation($"Data: {data}");
                data.Reevaluation_Status = EStatusSubmittion.PENDING;  //Lúc mới tạo là PENDING
                //data.StudentsId = data.Reevaluation_StudentId;
                await _context.Reevaluations.AddAsync(data);
                await _context.SaveChangesAsync();
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Tạo đơn phúc khảo thành công",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Có lỗi trong quá trình tạo đơn phúc khảo: ", ex.Message);
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình tạo đơn phúc khảo!",
                    Data = null
                };
            }
        }

        public Task<ApiResponse<string>> DeleteReevaluationAsync(Guid reevaluationId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStatus(EStatusSubmittion eStatus)
        {
            try
            {
                var reevaluations = await _context.Reevaluations
                    .Where(r => r.Reevaluation_Status == eStatus)
                    .Include(r => r.Students)
                    .Include(r => r.Teacher)
                    .Include(r => r.Subjects).ToListAsync();
                var response = _mapper.Map<List<ReevaluationResponse>>(reevaluations);
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "200",
                    Data = response,
                    Message = "Lấy danh sách đơn Phúc khảo thành công!"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình tạo đơn phúc khảo!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStatusAndYear(EStatusSubmittion eStatus, string year, string? query = null)
        {
            try
            {
                
                var reevaluations = await _context.Reevaluations
                    .Where(r => r.Reevaluation_Status == eStatus && r.Reevaluation_Year == year && (query == null || r.Students.Fullname.Contains(query)))
                    .Include(r => r.Students)
                    .Include(r => r.Subjects)
                    .Include(r => r.Teacher)
                    .ToListAsync();
                var response = _mapper.Map<List<ReevaluationResponse>>(reevaluations);
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "200",
                    Data = response,
                    Message = "Lấy danh sách đơn Phúc khảo thành công!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "500",
                    Message = "Lấy dữ liệu không thành công!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByStudent(Guid studentId, EStatusSubmittion eStatus)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId.ToString());
                if (student == null)
                {
                    return new ApiResponse<List<ReevaluationResponse>>()
                    {
                        Code = "404",
                        Message = "Học sinh không tồn tại!",
                        Data = null
                    };
                }

                var reevaluations = _context.Reevaluations.Where(r => r.StudentsId == studentId.ToString() && (eStatus == null || r.Reevaluation_Status == eStatus)).ToListAsync();
                var response = _mapper.Map<List<ReevaluationResponse>>(reevaluations);

                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "200",
                    Data = response,
                    Message = "Lấy danh sách đơn Phúc khảo thành công!"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình tạo đơn phúc khảo!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ReevaluationResponse>>> GetAllReevaluationByTeacher(Guid teacherId, EStatusSubmittion eStatus, string year)
        {
            try
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(s => s.Id == teacherId.ToString());
                if (teacher == null)
                {
                    return new ApiResponse<List<ReevaluationResponse>>()
                    {
                        Code = "404",
                        Message = "Giáo viên không tồn tại!",
                        Data = null
                    };
                }

                var reevaluations = await _context.Reevaluations.Where(r => r.TeacherId == teacherId.ToString()&& r.Reevaluation_Year == year && (eStatus == null || r.Reevaluation_Status == eStatus))
                    .Include(r => r.Students.ClassStudents)
                    .Include(r => r.Subjects)
                    .Include(r => r.Teacher)
                    .ToListAsync();
                var response = _mapper.Map<List<ReevaluationResponse>>(reevaluations);

                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "200",
                    Data = response,
                    Message = "Lấy danh sách đơn Phúc khảo thành công!"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ReevaluationResponse>>()
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình tạo đơn phúc khảo!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ReevaluationResponse>> GetReevaluationByIdAsync(Guid reevaluationId)
        {
            try
            {
                var reevaluation = await _context.Reevaluations
                    .Include(r => r.Students)
                    .Include(r => r.Subjects)
                    .Include(r => r.Teacher)
                    .FirstOrDefaultAsync(r => r.Reevaluation_Id == reevaluationId);

                if (reevaluation == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Message = "Không tìm thấy đơn phúc khảo nào!",
                        Data = null
                    };
                }
                var response = _mapper.Map<ReevaluationResponse>(reevaluation);
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Lấy đơn phúc khảo thành công!",
                    Data = response,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReevaluationResponse>()
                {
                    Message = ex.Message,
                    Code = "500",
                    Data = null
                };
            }
        }

        public Task<ApiResponse<List<ReevaluationResponse>>> GetReevaluationListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<ReevaluationsOfStudentResponse>>> GetReevaluationsOfStudent(string studentId, List<string> listYear)
        {
            //listYear = ['2024-2025', '2025-2026']
            try
            {
                var existStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
                if(existStudent == null)
                {
                    return new ApiResponse<List<ReevaluationsOfStudentResponse>>()
                    {
                        Code = "404",
                        Message = "Học sinh không tồn tại!",
                        Data = null
                    };
                }

                var reevaluationResponse = new ReevaluationsOfStudentResponse();
                var result = new List<ReevaluationsOfStudentResponse>();
                foreach (var year in listYear) {
                    var reevaluations = await _context.Reevaluations
                        .Where(r => r.StudentsId == studentId && r.Reevaluation_Year == year)
                        .Include(r => r.Students)
                        .Include(r => r.Subjects)
                        .Include(r => r.Teacher)
                        .ToListAsync();
                    reevaluationResponse.Year = year;
                    reevaluationResponse.Reevaluations = _mapper.Map<List<ReevaluationResponse>>(reevaluations);
                    result.Add(reevaluationResponse);
                }
                
                return new ApiResponse<List<ReevaluationsOfStudentResponse>>()
                {
                    Code = "200",
                    Message = "Lấy danh sách phúc khảo của học sinh thành công!",
                    Data = result
                };
            }
            catch(Exception e)
            {
                return new ApiResponse<List<ReevaluationsOfStudentResponse>>()
                {
                    Code = "500",
                    Message = "Có lỗi trong quá trình lấy danh sách phúc khảo của học sinh!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationAsync(Guid ReevaluationId, ReevaluationRequest request)
        {
            try
            {
                DateTime now = DateTime.Now;
                string editorId = _currentUserService.GetUserId();
                var editor = await _context.Users.FirstOrDefaultAsync(s => s.Id == editorId.ToString());
                if (editor == null)
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Message = "Tài khoản không tồn tại!",
                        Data = null
                    };

                var reevaluation = await _context.Reevaluations.FirstOrDefaultAsync(r => r.Reevaluation_Id == ReevaluationId);
                if (reevaluation == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Data = null,
                        Message = "Đơn phúc khảo không tồn tại!"
                    };
                }
                else
                {
                    reevaluation.Reevaluation_Status = request.Reevaluation_Status ?? EStatusSubmittion.CANCELED;
                    reevaluation.Reevaluation_AfterScore = request.Reevaluation_AfterScore;
                    reevaluation.Reevaluation_TimestampEdit = now;
                    reevaluation.TeacherId = editorId;
                    await _context.SaveChangesAsync();
                }
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Cập nhật trạng thái thành công!",
                    Data = _mapper.Map<ReevaluationResponse>(reevaluation)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi trong quá trình cập nhật: {ex.Message}");
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Duyệt đơn phúc khảo, chỉ admin mới có chức năng này.
        public async Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationStatus(EStatusSubmittion eStatus, Guid ReevaluationId)
        {
            try
            {
                DateTime now = DateTime.Now;
                string approverId = _currentUserService.GetUserId();
                var admin = await _context.SystemAdmins.FirstOrDefaultAsync(s => s.Id == approverId.ToString());
                if (admin == null)
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Message = "Tài khoản không tồn tại!",
                        Data = null
                    };

                var reevaluation = await _context.Reevaluations.FirstOrDefaultAsync(r => r.Reevaluation_Id == ReevaluationId);
                if (reevaluation == null)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Data = null,
                        Message = "Đơn phúc khảo không tồn tại!"
                    };
                }
                else
                {

                    //Lấy cột điểm ra để tiến hành cập nhật timestamp lại.
                    var student_score = await _context.Student_Score.FirstOrDefaultAsync(sc => sc.StudentId == reevaluation.StudentsId 
                                && sc.SubjectId == reevaluation.SubjectsId 
                                && sc.Year == reevaluation.Reevaluation_Year 
                                && sc.Semester == reevaluation.Reevaluation_Semester);
                    
                    //switch (reevaluation.Reevaluation_ScoreColumn)
                    //{
                    //    case "OralScore": student_score.OralScore_Timestamp = DateTime.Now; break;
                    //    case "QuizScore": student_score.QuizScore_Timestamp = DateTime.Now; break;
                    //    case "TestScore": student_score.TestScore_Timestamp = DateTime.Now; break;
                    //    case "MidTermScore": student_score.MidTermScore_Timestamp = DateTime.Now; break;
                    //    case "FinalExamScore": student_score.FinalExamScore_Timestamp = DateTime.Now; break;
                    //}
                    // Thêm cập nhật thêm một trường thời hạn phúc khảo.
                    reevaluation.Reevaluation_Status = eStatus;
                    reevaluation.Reevaluation_TimestampApprove = now;
                    reevaluation.Reevaluation_ApporverId = approverId;
                    await _context.SaveChangesAsync();
                }
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Cập nhật trạng thái thành công!",
                    Data = _mapper.Map<ReevaluationResponse>(reevaluation)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Có lỗi trong quá trình cập nhật: {ex.Message}");
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ReevaluationResponse>> UpdateReevaluationStatusAndUpdateScore(string year, int semester, double score, Guid ReevaluationId)
        {
            try
            {
                DateTime now = DateTime.Now;
                string editorId = _currentUserService.GetUserId();
                var reevaluation = await _context.Reevaluations.FirstOrDefaultAsync(r => r.Reevaluation_Id == ReevaluationId); 
                if (reevaluation == null || reevaluation.Reevaluation_Status != EStatusSubmittion.SUBMITTED || reevaluation.Reevaluation_Status != EStatusSubmittion.EDITTING)
                {
                    return new ApiResponse<ReevaluationResponse>()
                    {
                        Code = "404",
                        Data = null,
                        Message = "Đơn phúc khảo không tồn tại hoặc chưa được duyệt!"
                    };
                }
                else
                {
                    //Tiến hành cập nhật điểm cho học sinh và cập nhật lại Đơn phúc khảo để lưu vết.
                    try
                    {
                        UpdateScoreOfSubjectRequest request = new UpdateScoreOfSubjectRequest();
                        switch (reevaluation.Reevaluation_ScoreColumn)
                        {
                            case "OralScore": request.OralScore = score; break; 
                            case "OralScore1": request.OralScore1 = score; break; 
                            case "OralScore2": request.OralScore2 = score; break; 
                            case "OralScore3": request.OralScore3 = score; break; 
                            case "QuizScore": request.QuizScore = score; break; 
                            case "TestScore": request.TestScore = score; break; 
                            case "MidTermScore": request.MidTermScore = score; break; 
                            case "FinalExamScore": request.FinalExamScore = score; break; 
                        }

                        var scoreUpdated = await _scoreService.UpdateScoreOfSubject(reevaluation.StudentsId, year, semester, reevaluation.SubjectsId, request);
                        reevaluation.Reevaluation_AfterScore = score;
                        reevaluation.Reevaluation_Status = EStatusSubmittion.COMPLETED;
                        reevaluation.Reevaluation_TimestampEdit = now;
                        reevaluation.TeacherId = editorId.ToString();
                        _context.Reevaluations.Update(reevaluation);
                        await _context.SaveChangesAsync();

                        return new ApiResponse<ReevaluationResponse>()
                        {
                            Code = "200",
                            Message = "Cập nhật thông tin điểm số phúc khảo thành công!",
                            Data = null
                        };
                    }
                    catch (Exception ex) {
                        return new ApiResponse<ReevaluationResponse>()
                        {
                            Code = "500",
                            Message = "Cập nhật thông tin điểm số phúc khảo không thành công!",
                            Data = null
                        };
                    }

                }
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "200",
                    Message = "Cập nhật trạng thái thành công!",
                    Data = _mapper.Map<ReevaluationResponse>(reevaluation)
                };
            }
            catch
            {
                return new ApiResponse<ReevaluationResponse>()
                {
                    Code = "500",
                    Message = "Có lỗi khi cập nhật thông tin phúc khảo!",
                    Data = null
                };
            }
        }
    }
  
}
