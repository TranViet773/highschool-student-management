using AutoMapper;
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
    }
}
