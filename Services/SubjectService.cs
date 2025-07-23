using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ILogger<SubjectService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SubjectService(ILogger<SubjectService> logger, ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _logger = logger;   
            _mapper = mapper;
        }
        public async Task<ApiResponse<SubjectResponse>> addSubjectAsync(SubjectRequest request)
        {
            _logger.LogInformation("Create a new Subject!");
            var isExisted = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Name == request.Subject_Name);
            if (isExisted is not null)
            {
                _logger.LogError("Subject name already exists!");
                return new ApiResponse<SubjectResponse>
                {
                    Code = "400",
                    Message = "Subject name already exists!",
                    Data = null
                };
            }
            var result = _mapper.Map<Subjects>(request);
            await _context.Subjects.AddAsync(result);
            await _context.SaveChangesAsync();
            return new ApiResponse<SubjectResponse>
            {
                Code = "200",
                Message = "Create new Subject successfully!",
                Data = _mapper.Map<SubjectResponse>(result)
            };
        }

        public async Task<ApiResponse<SubjectResponse>> AssignTeachersToSubject(Guid subjectId, string teacherId, Guid classId, string year, string? semester)
        {
            _logger.LogInformation("Assign Teachers to Subject!");
            if (semester == null)
                semester = "all";
            var teaching = new Teacher_Subject
            {
                Subject_Id = subjectId,
                Class_Id = classId,
                Year = year,
                Teacher_Id = teacherId,
                Semester = semester
            };
            try
            {
                await _context.TeacherSubjects.AddAsync(teaching);
                await _context.SaveChangesAsync();
                return new ApiResponse<SubjectResponse>
                {
                    Code = "200",
                    Message = "Assign teacher to subject is succesfully!",
                    Data = null
                };
            }
            catch (Exception ex) {
                return new ApiResponse<SubjectResponse>
                {
                    Code = "500",
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<SubjectResponse>>> getAll()
        {
            var result = await _context.Subjects.ToListAsync();
            return new ApiResponse<List<SubjectResponse>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<List<SubjectResponse>>(result)
            };
        }

        public async Task<ApiResponse<SubjectResponse>> getSubjectById(Guid id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Id == id);
            if (subject == null) {
                _logger.LogError("Do not found!");
                return new ApiResponse<SubjectResponse>
                {
                    Code = "404",
                    Message = "Do not found!",
                    Data = null
                };
            }
            return new ApiResponse<SubjectResponse>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<SubjectResponse>(subject)
            };
        }

        public async Task<ApiResponse<ListTeacherBySubjectsResponse>> GetTeacherBySubjects(string classId, int semester, string year)
        {
            try
            {
                var teacherSubjects = await _context.TeacherSubjects
                    .Where(ts => ts.Year == year && ts.Class_Id.ToString() == classId)
                    .Include(ts => ts.Subjects)
                    .Include(ts => ts.Teacher)
                    .Select(ts => new TeacherSubjectDto
                    {
                        SubjectId = ts.Subject_Id.ToString().ToLower(),
                        SubjectName = ts.Subjects.Subject_Name,
                        TeacherId = ts.Teacher.Id,
                        TeacherName = ts.Teacher.Fullname
                    })
                    .ToListAsync();
                ListTeacherBySubjectsResponse response = new ListTeacherBySubjectsResponse();
                response.TeacherSubjects = teacherSubjects;
                response.year = year;
                response.semester = semester;
                return new ApiResponse<ListTeacherBySubjectsResponse>
                {
                    Code = "200",
                    Message = "Lấy dữ liệu thành công!",
                    Data = response
                };
            }
            catch (Exception ex) {
                return new ApiResponse<ListTeacherBySubjectsResponse>
                {
                    Code = "500",
                    Message = "Có lỗi khi lấy dữ liệu!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<ListTeachingByGradeAndSubjectResponse>> GetTeachingByGradeAndSubject(string grade, string year, string subjectId)
        {
            try
            { //grade: 10, 11, 12
                var existSubject = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Id.ToString() == subjectId);
                if(existSubject == null)
                {
                    return new ApiResponse<ListTeachingByGradeAndSubjectResponse>
                    {
                        Code = "404",
                        Data = null,
                        Message = "Không tìm thấy môn học."
                    };
                }
                var result = new ListTeachingByGradeAndSubjectResponse();
                result.Year = year.ToString();
                result.Grade = int.Parse(grade);
                var listTeaching = await _context.TeacherSubjects
                        .Where(ts => ts.Subject_Id.ToString() == subjectId 
                            && ts.Year == year 
                            && ts.Classes.Classes_Code.Substring(0, 2) == grade)
                        .Select(ts => new TeachingByGradeAndSubjectResponse { 
                            ClassCode = ts.Classes.Classes_Code,
                            ClassId = ts.Class_Id.ToString(),
                            ClassName = ts.Classes.Classes_Name,
                            TeacherId = ts.Teacher.Id,
                            TeacherCode = ts.Teacher.Code,
                            TeacherName = ts.Teacher.Fullname
                        })
                        .ToListAsync();

                result.TeachingByGradeAndSubjectResponse = listTeaching;

                return new ApiResponse<ListTeachingByGradeAndSubjectResponse>
                {
                    Code = "200",
                    Message = "",
                    Data = result
                };

            }catch(Exception e)
            {
                return new ApiResponse<ListTeachingByGradeAndSubjectResponse>
                {
                    Code = "500",
                    Data = null,
                    Message = "Có lỗi trong quá trình lấy danh sách giảng dạy."
                };
            }

        }

        public async Task<ApiResponse<List<ListTeachingByTeacherResponse>>> GetTeachingByTeacher(string year, int? semester)
        {
            try
            {
                var result = await _context.Teachers
                        .Select(t => new ListTeachingByTeacherResponse
                        {
                            TeacherCode = t.Code,
                            TeacherId = t.Id,
                            TeacherName = t.Fullname,
                            TeachingByTeacher = 
                                t.TeacherSubject.Where(ts => ts.Year == year)
                                .Select(ts => new TeachingByTeacherResponse
                                {
                                    ClassCode = ts.Classes.Classes_Code,
                                    ClassName = ts.Classes.Classes_Name,
                                    //SubjectId = ts.Subjects.Subject_Id,
                                    SubjectName = ts.Subjects.Subject_Name,
                                }).ToList()
                        })
                        .ToListAsync();

                return new ApiResponse<List<ListTeachingByTeacherResponse>>
                {
                    Code = "200",
                    Message = "Lấy danh sách Teaching - Teacher thành công!",
                    Data = result
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<List<ListTeachingByTeacherResponse>>
                {
                    Code = "200",
                    Message = "Có lỗi khi lấy danh sách Teaching của teacher.",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<String>> removeSubjectAsync(Guid id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Subject_Id == id);
            if (subject is null)
            {
                return new ApiResponse<String>
                {
                    Code = "404",
                    Message = "Do not found!",
                    Data = ""
                };
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return new ApiResponse<String>
            {
                Code = "200",
                Message = "Successfully!",
                Data = null
            }; 

        }

        public async Task<ApiResponse<SubjectResponse>> UnassignTeachersFromSubject(Guid subjectId, string teacherId, Guid classId)
        {
            var teaching = await _context.TeacherSubjects.FirstOrDefaultAsync(t => t.Teacher_Id == teacherId  && t.Subject_Id == subjectId && t.Class_Id == classId);
            if (teaching is null)
            {
                return new ApiResponse<SubjectResponse>
                {
                    Code = "404",
                    Message = "Do not found teaching",
                    Data = null
                };
            }
            try
            {
                _context.TeacherSubjects.Remove(teaching);
                await _context.SaveChangesAsync();
                return new ApiResponse<SubjectResponse>
                {
                    Code = "200",
                    Message = "Removing Teaching was successful!",
                    Data = null
                };
            }
            catch (Exception ex) {
                _logger.LogInformation("An error occurred while removing Teaching");
                return new ApiResponse<SubjectResponse>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public Task<ApiResponse<SubjectResponse>> updateSubjectAsync(SubjectRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<SubjectResponse>> UpdateTeachersForSubject(Guid subjectId, string teacherId, Guid ClassId, string year)
        {
            var teaching = await _context.TeacherSubjects.FirstOrDefaultAsync(t => t.Subject_Id == subjectId && t.Class_Id == ClassId);
            if (teaching == null) {
                return new ApiResponse<SubjectResponse> {
                    Code = "404",
                    Message = "Teaching not found!",
                    Data = null
                };
            }
            try
            {
                _context.TeacherSubjects.Remove(teaching);
                await _context.SaveChangesAsync();
                var newTeaching = new Teacher_Subject
                {
                    Subject_Id = subjectId,
                    Class_Id = ClassId,
                    Teacher_Id = teacherId,
                    Year = year,
                    Semester = "all"
                };

                _context.TeacherSubjects.Add(newTeaching);
                await _context.SaveChangesAsync();

                return new ApiResponse<SubjectResponse>
                {
                    Code = "200",
                    Message = "Updating teaching was successfully!",
                    Data = null
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ApiResponse<SubjectResponse> {
                    Code = "500",
                    Message = "An error occurred while updating Teaching!",
                    Data = null
                };
            }
        }
    }
}
