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
            request.Classes_Code = $"{request.Classes_Name}_{int.Parse(request.Year.Substring(2, 2))}".ToUpper(); // đnag giả định, đầu vào năm hiện tại => +1 để tạo lớp
            var isExisted = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Code == request.Classes_Code);
            if (isExisted != null)
            {
                _logger.LogError("Lớp đã tồn tại");
                return null;
            }

            var result_class = _mapper.Map<Classes>(request); 
            if(request.Teacher_Id != Guid.Empty)
            {
                var teacherId = request.Teacher_Id.ToString(); // Chuyển đổi từ Guid sang string
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
                var isAdvisor = await _context.TeacherClasses
                    .AnyAsync(t => t.Teacher_Id == teacherId && t.Year == request.Year);

                if (isAdvisor)
                {
                    _logger.LogError("Giáo viên đã làm chủ nhiệm!");
                    return null;
                }

                var teaching = _mapper.Map<Teacher_Class>(request);
                teaching.Class_Id = result_class.Classes_Id;
                teacher.isAdvisor = true;
                teacher.timeAdvisor = DateTime.Now.Year.ToString();
                await _context.TeacherClasses.AddAsync(teaching);
            }

            try
            {

                await _context.Classes.AddAsync(result_class);
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

        public async Task<ApiResponse<ClassResponse>> AddStudentToClass(string studentCode, Guid classId, string year)
        {
            try
            {
                var student = await _context.Users.OfType<Students>().FirstOrDefaultAsync(s => s.Code == studentCode);
                var isBelongClass = await _context.ClassStudents.AnyAsync(c => c.Student_Id == student.Id && c.Year == year);
                if (isBelongClass)
                {
                    return new ApiResponse<ClassResponse>
                    {
                        Code = "500",
                        Message = $"Học sinh có mã số {studentCode} đang tồn tại trong lớp khác trong năm học {year}!",
                        Data = null,
                    };
                }
                var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
                //var year = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";
                var result = new Class_Student
                {
                    Class_Id = classId,
                    Student_Id = student.Id,
                    Year = year
                };

                Class.Classes_Quantity += 1;
                await _context.ClassStudents.AddAsync(result);
                await _context.SaveChangesAsync();
                return new ApiResponse<ClassResponse>
                {
                    Code = "200",
                    Message = "Successfully!",
                    Data = null
                };
            }catch(Exception e)
            {
                Console.Write("Lỗi khi thêm học sinh vào lớp: ", e.Message);
                return new ApiResponse<ClassResponse>
                {
                    Code = "500",
                    Message = "Có lỗi khi thêm học sinh vào lớp.",
                    Data = null
                };
            }

        }

        public async Task<ApiResponse<string>> ChangeStudentToClass(string studentCode, Guid classId, string year)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Code == studentCode);
            var classStudent = new Class_Student();
            classStudent = await _context.ClassStudents.FirstOrDefaultAsync(u => u.Year == year && u.Student_Id == student.Id);
            if (classStudent != null)
            {
                var oldClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classStudent.Class_Id);
                oldClass.Classes_Quantity -= 1;
                _context.ClassStudents.Remove((Class_Student)classStudent);
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
            //Cập nhật số lượng
            var newClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
            newClass.Classes_Quantity += 1;
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
            //string year = "";
            var year = $"20{Class.Classes_Code.Substring(Class.Classes_Code.Length - 2, 2)}-20{int.Parse(Class.Classes_Code.Substring(Class.Classes_Code.Length - 2, 2))+1}";
            var class_student = await _context.ClassStudents.FirstOrDefaultAsync(cs => cs.Year == year && cs.Class_Id == classId && cs.Student_Id == student.Id);
            _context.ClassStudents.Remove(class_student);
            Class.Classes_Quantity -= 1;
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
            year = year.Substring(2, 2); 
            var classes = await _context.Classes.Where(c => c.Classes_Code.Substring(c.Classes_Code.Length-2, 2) == year).ToListAsync();
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
            var result = _mapper.Map<ClassResponse>(Class);

            var teacher_Class = await _context.TeacherClasses.FirstOrDefaultAsync(tc => tc.Class_Id.ToString() == id);
            if (teacher_Class != null)
            {
                var advisor = await _context.Users.OfType<Teacher>().FirstOrDefaultAsync(t => t.Id == teacher_Class.Teacher_Id);
                var advisor_name = advisor?.Fullname;
                result.advisor = advisor_name;
            }

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

            result.Year = year;
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

        public async Task<ApiResponse<List<ClassResponse>>> GetClassByTeacher(string teacherId, string? year, string? semester)
        {
            var teaching = await _context.TeacherSubjects
                 .Where(t =>
                     (year != null ? t.Year == year : true) &&
                     (semester != null ? t.Semester == semester : true) &&
                     t.Teacher_Id == teacherId)
                 .Select(t => new ClassResponse
                 {
                     Classes_Id = t.Classes.Classes_Id,
                     Classes_Name = t.Classes.Classes_Name,
                     Subject_Name = t.Subjects.Subject_Name,
                     Subject_Id = t.Subjects.Subject_Id.ToString(),
                     Year = t.Year,
                     Semester = t.Semester
                 })
                 .ToListAsync(); 

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

        public async Task<ApiResponse<string>> UpdateAdvisorForClass(string teacherId, Guid classId, string year, string semester)
        {
            var isAdvisor = await _context.TeacherClasses.FirstOrDefaultAsync(t => t.Teacher_Id == teacherId && t.Year == year && t.Semester == semester);
            if (isAdvisor != null) //Đang làm chủ nhiệm một lớp khác.
            {
                return new ApiResponse<string>
                {
                    Code = "500",
                    Message = "",
                    Data = "This teacher is already a homeroom teacher for another class."
                };
            }

            var teacherClass = await _context.TeacherClasses.FirstOrDefaultAsync(t => t.Class_Id == classId); // Check lại
            if (teacherClass != null)
            {
                _context.TeacherClasses.Remove(teacherClass);
                await _context.SaveChangesAsync();
            }
            Teacher_Class teacher_Class = new Teacher_Class
            {
                Class_Id = classId,
                Teacher_Id = teacherId,
                Semester = "all",
                Year = year
            };
            await _context.TeacherClasses.AddAsync(teacher_Class);
            await _context.SaveChangesAsync();
            return new ApiResponse<string>
            {
                Code = "200",
                Message = "",
                Data = "Update success"
            };
        }

        public async Task<ApiResponse<string>> AdvanceStudentsToNextClass(Guid oldClassId, Guid newClassId, string year)
        {
            try
            {
                var existingOldClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == oldClassId);
                if(existingOldClass == null)
                {
                    return new ApiResponse<string>
                    {
                        Code = "404",
                        Message = "Id lớp cũ không tồn tại!",
                        Data = null
                    };
                }

                var existingNewClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == newClassId);
                if (existingNewClass == null)
                {
                    return new ApiResponse<string>
                    {
                        Code = "404",
                        Message = "Id lớp mới không tồn tại!",
                        Data = null
                    };
                }

                // Lấy danh sách mã học sinh từ lớp cũ theo năm học
                var studentCodes = await _context.ClassStudents
                    .Where(cs => cs.Class_Id == oldClassId && cs.Year == year)
                    .Select(cs => cs.Students.Code)
                    .ToListAsync();

                if (studentCodes == null || studentCodes.Count == 0)
                {
                    return new ApiResponse<string>
                    {
                        Code = "404",
                        Message = "Không tìm thấy học sinh trong lớp cũ",
                        Data = null
                    };
                }

                // Xác định năm kế tiếp
                var yearParts = year.Split('-');
                if (yearParts.Length != 2 || !int.TryParse(yearParts[0], out int startYear))
                {
                    return new ApiResponse<string>
                    {
                        Code = "400",
                        Message = "Năm học không hợp lệ",
                        Data = null
                    };
                }

                string nextYear = $"{startYear + 1}-{startYear + 2}";

                // Thêm từng học sinh vào lớp mới
                foreach (var studentCode in studentCodes)
                {
                    try
                    {
                        var isPassedClass = await _context.Evaluations.FirstOrDefaultAsync(e => e.Student.Code == studentCode && e.isPassClass == true);// Để đảm bảo Chỉ những trường hợp đủ đk lên lớp mới dc lên(khác vs false và null);
                        var resultAddStudent = await AddStudentToClass(studentCode, newClassId, nextYear);
                        if(resultAddStudent.Code == "500")
                        {
                            return new ApiResponse<string>
                            {
                                Code = "500",
                                Data = null,
                                Message = resultAddStudent.Message
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Lỗi khi thêm học sinh {studentCode} vào lớp mới: {ex.Message}");
                        return new ApiResponse<string>
                        {
                            Code = "500",
                            Message = $"Lỗi khi thêm học sinh {studentCode} vào lớp mới",
                            Data = ex.Message
                        };
                    }
                }

                existingOldClass.Classes_NextClassId = newClassId.ToString();
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    Code = "200",
                    Message = "Chuyển học sinh sang lớp mới thành công",
                    Data = $"Đã chuyển {studentCodes.Count} học sinh sang lớp mới"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi tổng quát: {ex.Message}");
                return new ApiResponse<string>
                {
                    Code = "500",
                    Message = "Lỗi khi nâng lớp học sinh",
                    Data = ex.Message
                };
            }
        }


        public async Task<ApiResponse<string>> CreateClassesToAdvance(string year, List<string> codeClasses)
        {
            try
            {
                // Parse "2024-2025" → startYear = 2024
                var yearParts = year.Split('-');
                if (yearParts.Length != 2 || !int.TryParse(yearParts[0], out int startYear))
                {
                    return new ApiResponse<string>
                    {
                        Code = "400",
                        Data = null,
                        Message = "Năm học không hợp lệ"
                    };
                }

                // Tính năm học kế tiếp: "2025-2026"
                string nextYear = $"{startYear + 1}-{startYear + 2}";

                foreach (var codeClass in codeClasses)
                {
                    // VD: "10A4_24" hoặc "11B1_24"
                    var parts = codeClass.Split('_');
                    if (parts.Length != 2) continue;

                    string rawName = parts[0]; // "10A4"

                    // Tách khối và hậu tố
                    string prefix = new string(rawName.TakeWhile(char.IsDigit).ToArray()); // "10"
                    string suffix = new string(rawName.SkipWhile(char.IsDigit).ToArray()); // "A4"

                    if (!int.TryParse(prefix, out int currentGrade)) continue;

                    if (currentGrade >= 12) continue; // Bỏ qua lớp 12

                    int nextGrade = currentGrade + 1; // 10 -> 11, 11 -> 12
                    string newClassName = $"{nextGrade}{suffix}"; // VD: "11A4"

                    var createClassRequest = new ClassRequest
                    {
                        Classes_Name = newClassName,
                        Year = nextYear
                    };

                    var result = await AddClass(createClassRequest);

                    if (result == null)
                    {
                        return new ApiResponse<string>
                        {
                            Code = "500",
                            Data = null,
                            Message = $"Tạo lớp thất bại: {newClassName}"
                        };
                    }
                }

                return new ApiResponse<string>
                {
                    Code = "200",
                    Data = "Tạo lớp thành công",
                    Message = "Đã tạo xong các lớp để nâng lớp"
                };
            }
            catch (Exception e)
            {
                return new ApiResponse<string>
                {
                    Code = "500",
                    Data = null,
                    Message = "Có lỗi khi tạo lớp để nâng lớp: " + e.Message
                };
            }
        }

        public async Task<ApiResponse<string>> AdvanceClassForStudents(IEnumerable<string> studentCodes, Guid classId, string year)
        {
            // Tính nextYear từ year
            var yearParts = year.Split('-');
            if (yearParts.Length != 2 || !int.TryParse(yearParts[0], out int fromYear))
            {
                return new ApiResponse<string>
                {
                    Code = "400",
                    Message = "Năm học không hợp lệ.",
                    Data = ""
                };
            }
            var nextYear = $"{fromYear + 1}-{fromYear + 2}";

            var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
            if (classEntity == null)
            {
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "Lớp mới không tồn tại.",
                    Data = ""
                };
            }

            int successCount = 0;
            foreach (var studentCode in studentCodes)
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.Code == studentCode);
                if (student == null) continue;

                // Kiểm tra học sinh đã có trong lớp mới ở năm tiếp theo chưa
                var existed = await _context.ClassStudents.AnyAsync(cs =>
                    cs.Student_Id == student.Id && cs.Class_Id == classId && cs.Year == nextYear);
                if (existed) continue;

                var newClassStudent = new Class_Student
                {
                    Student_Id = student.Id,
                    Class_Id = classId,
                    Year = nextYear
                };

                await _context.ClassStudents.AddAsync(newClassStudent);
                classEntity.Classes_Quantity += 1;
                successCount++;
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Code = "200",
                Message = $"Đã nâng lớp thành công cho {successCount} học sinh.",
                Data = ""
            };
        }

        public async Task<ApiResponse<string>> ChangeStudentsToClass(List<string> studentCodes, Guid classId, string year)
        {
            // Lấy danh sách học sinh tương ứng
            var students = await _context.Students
                .Where(s => studentCodes.Contains(s.Code))
                .ToListAsync();

            if (students.Count == 0)
            {
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "Không tìm thấy học sinh nào trong danh sách cung cấp.",
                    Data = ""
                };
            }

            // Lấy lớp mới
            var newClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classId);
            if (newClass == null)
            {
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "Lớp mới không tồn tại.",
                    Data = ""
                };
            }

            foreach (var student in students)
            {
                var oldClassStudent = await _context.ClassStudents
                    .FirstOrDefaultAsync(cs => cs.Student_Id == student.Id && cs.Year == year);

                if (oldClassStudent != null)
                {
                    var oldClass = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == oldClassStudent.Class_Id);
                    if (oldClass != null)
                    {
                        oldClass.Classes_Quantity = Math.Max(0, oldClass.Classes_Quantity - 1);
                    }

                    _context.ClassStudents.Remove(oldClassStudent);
                }

                // Thêm vào lớp mới
                var newClassStudent = new Class_Student
                {
                    Class_Id = classId,
                    Student_Id = student.Id,
                    Year = year
                };
                await _context.ClassStudents.AddAsync(newClassStudent);

                newClass.Classes_Quantity += 1;
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Code = "200",
                Message = "Chuyển học sinh thành công.",
                Data = ""
            };
        }
    }
}
