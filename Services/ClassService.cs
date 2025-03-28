using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;

namespace NL_THUD.Services
{
    public class ClassService : IClassService
    {
        private readonly ILogger<ClassService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ClassService(ApplicationDbContext context, IMapper mapper, ILogger<ClassService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ClassResponse> AddClass(ClassRequest request)
        {
            _logger.LogInformation("Create a new class!");
            request.Semester = "all";
            request.Classes_Quantity = "0";
            request.Classes_Code = $"{request.Classes_Name}_{request.Year.Substring(2,2)}".ToUpper();
            var isExisted = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Code == request.Classes_Code);
            if (isExisted != null)
            {
                _logger.LogError("Lớp đã tồn tại");
                return null;
            }

            var teacherId = request.Teacher_Id.ToString(); // Chuyển đổi từ Guid sang string
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
            var isAdvisor = await _context.TeacherClasses
                .AnyAsync(t => t.Teacher_Id == teacherId && t.Year == request.Year);

            if (isAdvisor)
            {
                _logger.LogError("Giáo viên đã làm chủ nhiệm!");
                return null;
            }

            var result_class = _mapper.Map<Classes>(request); 
            var teaching = _mapper.Map<Teacher_Class>(request);
            teaching.Class_Id = result_class.Classes_Id;
            teacher.isAdvisor = true;
            teacher.timeAdvisor = DateTime.Now.Year.ToString();
            try
            {

                await _context.Classes.AddAsync(result_class);
                await _context.TeacherClasses.AddAsync(teaching);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                throw;
            }

            _logger.LogInformation($"{nameof(AddClass)}");
            return _mapper.Map<ClassResponse>(result_class);
        }

        public async Task<ApiResponse<string>> AddStudentToClass(string studentCode, Guid classId)
        {
            var student = await _context.Users.OfType<Students>().FirstOrDefaultAsync(s => s.Code == studentCode);
            var isBelongClass = await _context.ClassStudents.AnyAsync(c => c.Student_Id == student.Id);
            if (isBelongClass)
            {
                return new ApiResponse<string>
                {
                    Code = "500",
                    Message = "Student is belong to another class!",
                    Data = ""
                };
            }
            var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
            var year = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
            var result = new Class_Student
            {
                Class_Id = classId,
                Student_Id = student.Id,
                Year = year
            };
            await _context.ClassStudents.AddAsync(result);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>
            {
                Code = "200",
                Message = "Successfully!",
                Data = ""
            };

        }

        public async Task<ApiResponse<string>> ChangeStudentToClass(string studentCode, Guid classId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Code == studentCode);
            var year = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
            var classStudent = new Class_Student();
            classStudent = await _context.ClassStudents.FirstOrDefaultAsync(u => u.Year == year && u.Student_Id == student.Id);
            if (classStudent != null)
            {
                _context.ClassStudents.Remove((Class_Student)classStudent);
                await _context.SaveChangesAsync();
            }
            else
            {
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "This class does not currently exist.",
                    Data = ""
                };
            }
            var newClassStudent = new Class_Student
            {
                Class_Id = classId,
                Student_Id = student.Id,
                Year = year
            };
            await _context.ClassStudents.AddAsync(newClassStudent);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>
            {
                Code = "200",
                Message = "Successfully!",
                Data = ""
            };
        }

        public async Task DeleteClass(string id)
        {
            var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id.ToString() == id);
            _context.Classes.Remove(Class);
            await _context.SaveChangesAsync();
        }

        public async Task<ApiResponse<string>> DeleteStudentToClass(string studentCode, Guid classId)
        {
            var student = await _context.Users.OfType<Students>().FirstOrDefaultAsync(s => s.Code == studentCode);
            var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
            var year = $"20{Class.Classes_Code.Substring(Class.Classes_Code.Length - 2, 2)}";
            var class_student = await _context.ClassStudents.FirstOrDefaultAsync(cs => cs.Year == year && cs.Class_Id == classId && cs.Student_Id == student.Id);
            _context.ClassStudents.Remove(class_student);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>
            {
                Code = "200",
                Message = "Successfully!",
                Data = ""
            };
        }

        public async Task<List<ClassResponse>> GetAllClasses(string year)
        {
            if(year == null)
                year = DateTime.Now.Year.ToString();
            var classes = await _context.Classes.Where(c => c.TeacherClass.Any(tc => tc.Year == year)).ToListAsync();
            return _mapper.Map<List<ClassResponse>>(classes);
        }

        public async Task<ClassResponse> GetClassByCode(string code)
        {
            var result = await _context.Classes.FirstOrDefaultAsync(u => u.Classes_Code == code);
            return _mapper.Map<ClassResponse>(result);
        }

        public async Task<ClassResponse> GetClassById(string id)
        {
            var Class = _context.Classes.FirstOrDefault(c => c.Classes_Id.ToString() == id);
            var teacher_Class = await _context.TeacherClasses.FirstOrDefaultAsync(tc => tc.Class_Id.ToString() == id);
            var advisor = await _context.Users.OfType<Teacher>().FirstOrDefaultAsync(t => t.Id == teacher_Class.Teacher_Id);
            var advisor_name = advisor?.Fullname; 
            if (Class == null)
            {
                _logger.LogError("Class doest not exist!");
                throw new NotImplementedException("Class does not exist!");
            }

            var sufyear = Class.Classes_Code.Substring(Class.Classes_Code.Length - 2, 2);
            var year = $"20{sufyear}-20{int.Parse(sufyear)+1}";
            var students = await _context.Students
                .Where(s => s.ClassStudents.Any(cs => cs.Class_Id.ToString() == id && cs.Year == year))
                .ToListAsync();

            var result = _mapper.Map<ClassResponse>(Class);

            //var subjectInfo = await _context.TeacherSubjects
            //        .Include(ts => ts.Subjects)
            //        .Where(ts => ts.Teacher_Id == teacher_Class.Teacher_Id
            //                  && ts.Year == teacher_Class.Year
            //                  && ts.Semester == teacher_Class.Semester
            //                  && ts.Class_Id.ToString() == id)
            //        .Select(ts => new
            //        {
            //            SubjectName = ts.Subjects.Subject_Name,
            //            SubjectId = ts.Subject_Id
            //        }).FirstOrDefaultAsync();

            //result.Subject_Name = subjectInfo?.SubjectName;
            //result.Subject_Id = subjectInfo?.SubjectId.ToString();
            result.advisor = advisor_name;
            result.Students = _mapper.Map<List<UserResponse>>(students);
            return result;
        }

        public async Task<ApiResponse<List<ClassResponse>>> GetClassesByGrade(string grade, string year)
        {
            var Class = await _context.Classes.Where(u => u.Classes_Code.Substring(0,2) == grade && 
                                                     u.Classes_Code.Substring(u.Classes_Code.Length - 2,2) == year.Substring(2,2)).ToListAsync();

            if (Class == null)
            {
                return new ApiResponse<List<ClassResponse>>
                {
                    Code = "404",
                    Message = "Do not found!",
                    Data = null
                };
            }
            return new ApiResponse<List<ClassResponse>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = _mapper.Map<List<ClassResponse>>(Class)
            };
        }

        public async Task<ApiResponse<List<ClassResponse>>> GetClassByTeacher(string teacherId, string year, string semester)
        {
           var teaching =  _context.TeacherSubjects.Where(t => t.Year == year && t.Semester == semester && t.Teacher_Id == teacherId)
                .Select(
                    t => new ClassResponse
                    {
                        Classes_Id = t.Classes.Classes_Id,
                        Classes_Name = t.Classes.Classes_Name,
                        Subject_Name = t.Subjects.Subject_Name,
                        Subject_Id = t.Subjects.Subject_Id.ToString(),
                        Year = year,
                        Semester = semester
                    }
               ).ToList();
            Console.WriteLine("Teaching: " + teaching);
            if (!teaching.Any()) {
                _logger.LogError("Teaching rỗng");
                return new ApiResponse<List<ClassResponse>>
                {
                    Code = "404",
                    Message = "Teaching was not found!",
                    Data = null,
                };
            }
            return new ApiResponse<List<ClassResponse>>
            {
                Code = "200",
                Message = "Getting Teaching was successful!",
                Data = teaching
            };
        }
    }
}
