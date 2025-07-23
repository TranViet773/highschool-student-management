using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Response;
using NL_THUD.Services.ServiceImpl;
using System.Security.Cryptography.Xml;

namespace NL_THUD.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentService> _logger;
        public StudentService(IMapper mapper, ApplicationDbContext context, ILogger<StudentService> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }
        public async Task<List<UserResponse>> getAllByClass(string codeClass)
        {
            var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Code == codeClass);
            var sufyear = codeClass.Substring(codeClass.Length - 2, 2);
            var year = $"20{sufyear}";
            var students = await _context.Students
                .Where(s => s.ClassStudents.Any(cs => cs.Class_Id == Class.Classes_Id && cs.Year == year))
                .ToListAsync();
            return _mapper.Map<List<UserResponse>>(students);
        }

        public async Task<ApiResponse<ClassOfStudentResponse>> GetAllClassByStudentId(string studentId)
        {
            try
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
                if (student == null)
                {
                    return new ApiResponse<ClassOfStudentResponse>
                    {
                        Code = "404",
                        Message = "Student not found",
                        Data = null
                    };
                }

                var classOfStudent = await _context.ClassStudents
                    .Where(c => c.Student_Id == studentId)
                    .Select(c => new ClassOfStudent
                    {
                        ClassId = c.Class_Id.ToString(),
                        Year = c.Year,
                        ClassName = c.Class.Classes_Name,
                        ClassCode = c.Class.Classes_Code
                    })
                    .ToListAsync();

                return new ApiResponse<ClassOfStudentResponse>
                {
                    Code = "200",
                    Message = "Success",
                    Data = new ClassOfStudentResponse
                    {
                        ClassOfStudents = classOfStudent
                    }
                };
             }
            catch (Exception e)
            {
                return new ApiResponse<ClassOfStudentResponse>
                {
                    Code = "500",
                    Message = $"Internal server error: {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<UserResponse>>> GetStudentsNotPassedClass(string year, int grade)
        {
            try
            {
                //year = "2024-2025" => nextYear = "2025-2026"var parts = year.Split('-');
                var parts = year.Split('-');
                string nextYear = year;
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int start) &&
                    int.TryParse(parts[1], out int end))
                {
                    nextYear = $"{start + 1}-{end + 1}";
                }

                var students = await _context.Students
                        .Where(s => s.Evaluations.Any(e => e.isPassClass == false && e.year == year && e.semester == 0))
                        .Select(s => new UserResponse
                        {
                            Id = Guid.Parse(s.Id),
                            FullName = s.Fullname,
                            Code = s.Code,
                            DoB = s.DoB,
                            Email = s.Email,
                            PhoneNumber = s.PhoneNumber,
                            Gender = s.Gender,
                            ClassCode = s.ClassStudents.Where(sc => sc.Year == year)
                                                        .Select(sc => sc.Class.Classes_Code)
                                                        .FirstOrDefault(),
                            NewClassCode = s.ClassStudents.Where(sc => sc.Year == nextYear)
                                              .Select(sc => sc.Class.Classes_Code)
                                              .FirstOrDefault()
                        })
                        .ToListAsync();


                return new ApiResponse<List<UserResponse>>
                {
                    Code = "200",
                    Message = "Lấy danh sách học sinh không lên lớp trong năm học thành công!",
                    Data = students
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<List<UserResponse>>
                {
                    Code = "500",
                    Message = "Có lỗi khi lấy danh sách học sinh không lên lớp!",
                    Data = null
                };
            }
        }

        public Task UpdateStudent(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
