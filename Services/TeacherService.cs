using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext _context;
        public TeacherService(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this._context = context;
        }

        public async Task<ApiResponse<List<AdvisorsInYearResponse>>> getAdvisorsInYear(string year)
        {
            var data = await _context.TeacherClasses
                        .Where(t => t.Year == year)
                        .Include(t => t.Teacher)
                        .Include(t => t.Class)
                        .ToListAsync();
            if (data == null)
            {
                return new ApiResponse<List<AdvisorsInYearResponse>>
                {
                    Code = "404",
                    Message = "Không tìm thấy chủ nhiệm nào.",
                    Data = null
                };
            }

            var result = data.Select(t => new AdvisorsInYearResponse
            {
                FullName = t.Teacher.Fullname,
                ClassName = t.Class.Classes_Name,
                Class_Id = t.Class.Classes_Id,
                ClassCode = t.Class.Classes_Code,
                year = t.Year,
                Id = t.Teacher.Id,
                Code = t.Teacher.Code,
            }).ToList();

            return new ApiResponse<List<AdvisorsInYearResponse>>
            {
                Code = "200",
                Message = "Lấy danh sách thành công.",
                Data = result
            };

        }

        public async Task<ApiResponse<List<ClassResponse>>> getListStudentAndClassByAdvisor(string teacherId)
        {
            try
            {
                var classTeacher = await _context.TeacherClasses
                    .Where(t => t.Teacher_Id == teacherId.ToString())
                    .ProjectTo<ClassResponse>(mapper.ConfigurationProvider)
                    .ToListAsync();
                if (classTeacher == null)
                {
                    return new ApiResponse<List<ClassResponse>>
                    {
                        Code = "404",
                        Message = "Class not found!",
                        Data = null
                    };
                }
                return new ApiResponse<List<ClassResponse>>
                {
                    Code = "200",
                    Message = "Lấy danh sách thành công!",
                    Data = classTeacher
                };
            }catch(Exception e)
            {
                return new ApiResponse<List<ClassResponse>>
                {
                    Code = "500",
                    Message = e.Message,
                    Data = null
                };
            }


        }

        public async Task<ApiResponse<ClassResponse>> getStudentAndClassByAdvisor(Guid teacherId, string year)
        {
            var classTeacher = await _context.TeacherClasses.FirstOrDefaultAsync(t => t.Teacher_Id == teacherId.ToString() && t.Year == year);
            if (classTeacher == null)
            {
                return new ApiResponse<ClassResponse>
                {
                    Code = "404",
                    Message = "Class not found!",
                    Data = null
                };
            }
            var classInfor = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classTeacher.Class_Id);
            var students = await _context.Students.Where(s => s.ClassStudents.Any(cs => cs.Class_Id == classInfor.Classes_Id && cs.Year == year)).ToListAsync();
            var classResponse = mapper.Map<ClassResponse>(classInfor);
            classResponse.Students = mapper.Map<List<UserResponse>>(students);
            classResponse.Year = year;
            return new ApiResponse<ClassResponse>
            {
                Code = "200",
                Message = "Successfully!",
                Data = classResponse
            };
        }

        //public Task<ApiResponse<ClassResponse>> getStudentAndClassBySubject(string classId, string year)
        //{
        //    var user
        //}

        public async Task<ApiResponse<UserResponse>> getTeacherBySubjectAndClass(Guid subjectId, Guid classId)
        {
            var teacherSubject = await _context.TeacherSubjects.FirstOrDefaultAsync(t => t.Class_Id == classId && t.Subject_Id == subjectId);
            if (teacherSubject == null) { 
                return new ApiResponse<UserResponse>
                {
                    Code = "404",
                    Message = "Teacher not found!",
                    Data = null
                };
            }
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherSubject.Teacher_Id);
            return new ApiResponse<UserResponse>
            {
                Code = "200",
                Message = "Successfully!",
                Data = mapper.Map<UserResponse>(teacher)
            };
        }

        public async Task<ApiResponse<List<TimeLineTeachingResponse>>> getTimeLineTeaching(string teacherId)
        {
            try
            {
                var existTeacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
                if (existTeacher == null)
                {
                    return new ApiResponse<List<TimeLineTeachingResponse>>
                    {
                        Code = "404",
                        Message = "Không tìm thấy giáo viên.",
                        Data = null
                    };
                }

                var groupedResult = await _context.TeacherSubjects
                    .Where(ts => ts.Teacher_Id == teacherId)
                    .Select(ts => new ClassResponse
                    {
                        Classes_Code = ts.Classes.Classes_Code,
                        Classes_Name = ts.Classes.Classes_Name,
                        Classes_Id = ts.Classes.Classes_Id,
                        Classes_Quantity = ts.Classes.Classes_Quantity.ToString(),
                        Subject_Id = ts.Subject_Id.ToString(),
                        Subject_Name = ts.Subjects.Subject_Name,
                        Year = ts.Year,
                        Semester = ts.Semester
                    })
                    .GroupBy(c => c.Year)
                    .Select(g => new TimeLineTeachingResponse
                    {
                        Year = g.Key,
                        classResponses = g.ToList()
                    })
                    .OrderByDescending(t => t.Year) // optional: newest year first
                    .ToListAsync();

                return new ApiResponse<List<TimeLineTeachingResponse>>
                {
                    Code = "200",
                    Message = "Thành công",
                    Data = groupedResult
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TimeLineTeachingResponse>>
                {
                    Code = "500",
                    Message = "Có lỗi khi lấy dòng thời gian của giáo viên.",
                    Data = null
                };
            }
        }
    }
}
