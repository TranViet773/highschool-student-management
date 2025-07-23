using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Services.ServiceImpl;
using Sprache;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NL_THUD.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _curentUserService;
        private readonly ILogger<AttendanceService> _logger;
        public AttendanceService(ApplicationDbContext context, IMapper mapper, ICurrentUserService curentUserService, ILogger<AttendanceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _curentUserService = curentUserService;
            _logger = logger;
        }
        public async Task<ApiResponse<AttendanceResponse>> CreateAttendance(AttendanceRequest request)
        {
            try
            {
                var newAttendance = _mapper.Map<Attendance>(request);
                newAttendance.Attendance_MarkedBy = _curentUserService.GetUserId();
                newAttendance.Attendance_Timestamp = DateTime.Now;
                var result = await _context.Attendances.AddAsync(newAttendance);
                await _context.SaveChangesAsync();
                return new ApiResponse<AttendanceResponse>
                {
                    Code = "200",
                    Message = "Create attendance was successfully!",
                    Data = _mapper.Map<AttendanceResponse>(result.Entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AttendanceResponse>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private (DateTime startOfWeek, DateTime endOfWeek) GetWeekRange(DateTime dayOfWeek)
        {
            // Tìm số thứ tự của ngày trong tuần (Thứ Hai = 1, Chủ Nhật = 0)
            int diffToMonday = (7 + (dayOfWeek.DayOfWeek - DayOfWeek.Monday)) % 7;

            // Ngày bắt đầu tuần (Thứ Hai)
            DateTime startOfWeek = dayOfWeek.AddDays(-1 * diffToMonday).Date;

            // Ngày kết thúc tuần (Thứ Bảy)
            DateTime endOfWeek = startOfWeek.AddDays(5); // Nếu muốn Chủ Nhật thì +6

            return (startOfWeek, endOfWeek);
        }

        public Task<ApiResponse<AttendanceResponse>> DeleteAttendance(AttendanceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendanceByClass(string year, Guid classId)
        {
            try
            {
                var attendances = await _context.ClassStudents
                                        .Where(cs => cs.Class_Id == classId && cs.Year == year)
                                        .SelectMany(cs => cs.Students.Attendances.Where(a => a.Attendance_Year == year)
                                            .Select(a => new AttendanceResponse
                                            {
                                                Attendance_Id = a.Attendance_Id,
                                                IsExcused = a.IsExcused,
                                                IsUnexcused = a.IsUnexcused,
                                                Attendance_Year = a.Attendance_Year,
                                                Attendance_Semester = a.Attendance_Semester,
                                                Attendance_Note = a.Attendance_Note,
                                                Attendance_Timestamp = a.Attendance_Timestamp,
                                                Attendance_MarkedBy = a.Attendance_MarkedBy,
                                                Attendance_Teacher = a.Teacher.Fullname,  // Lấy tên giáo viên
                                                Attendance_SubjectId = a.Attendance_SubjectId,
                                                Attendance_SubjectName = a.Attendance_SubjectName,
                                                StudentsId = a.StudentsId,
                                                Attendance_StudentName = a.Students.Fullname,
                                                Attendance_StudentCode = a.Students.Code 
                                            })
                                        ).ToListAsync();

                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "200",
                    Message = "Get All Attendances by classId was successfully!",
                    Data = _mapper.Map<List<AttendanceResponse>>(attendances)
                };
            }
            catch (Exception ex) {
                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public Task<ApiResponse<AttendanceResponse>> GetAllAttendances(AttendanceRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendancesByDate(string date, string year, Guid classId)
        {
            try
            {
                if(!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return await Task.FromResult(new ApiResponse<List<AttendanceResponse>>
                    {
                        Code = "500",
                        Message = "Ngày không hợp lệ!",
                        Data = null
                    });
                }

                var attendances = await _context.ClassStudents
                    .Where(cs => cs.Class_Id == classId && cs.Year == year)
                    .SelectMany(cs => cs.Students.Attendances
                        .Where(a => a.Attendance_Timestamp.Date == parsedDate.Date)
                        .Select(a => new AttendanceResponse
                        {
                            Attendance_Id = a.Attendance_Id,
                            IsExcused = a.IsExcused,
                            IsUnexcused = a.IsUnexcused,
                            Attendance_Year = a.Attendance_Year,
                            Attendance_Semester = a.Attendance_Semester,
                            Attendance_Note = a.Attendance_Note,
                            Attendance_Timestamp = a.Attendance_Timestamp,
                            Attendance_MarkedBy = a.Attendance_MarkedBy,
                            Attendance_Teacher = a.Teacher.Fullname,  // Lấy tên giáo viên
                            Attendance_SubjectId = a.Attendance_SubjectId,
                            Attendance_SubjectName = a.Attendance_SubjectName,
                            StudentsId = a.StudentsId,
                            Attendance_StudentName = a.Students.Fullname,
                            Attendance_StudentCode = a.Students.Code
                        }))
                    .ToListAsync();


                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "200",
                    Message = "Get All Attendances by classId was successfully!",
                    Data = _mapper.Map<List<AttendanceResponse>>(attendances)
                };
            }
            catch (Exception ex) {
                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendancesByWeek(string date, string year, Guid classId)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return await Task.FromResult(new ApiResponse<List<AttendanceResponse>>
                    {
                        Code = "500",
                        Message = "Ngày không hợp lệ!",
                        Data = null
                    });
                }
                var week = GetWeekRange(parsedDate);
                var attendances = await _context.ClassStudents
                                    .Where(cs => cs.Year == year && cs.Class_Id == classId)
                                    .SelectMany(cs => cs.Students.Attendances
                                        .Where(a => a.Attendance_Timestamp.Date >= week.startOfWeek.Date && a.Attendance_Timestamp.Date <= week.endOfWeek.Date)
                                        .Select(a => new AttendanceResponse
                                        {
                                            Attendance_Id = a.Attendance_Id,
                                            IsExcused = a.IsExcused,
                                            IsUnexcused = a.IsUnexcused,
                                            Attendance_Year = a.Attendance_Year,
                                            Attendance_Semester = a.Attendance_Semester,
                                            Attendance_Note = a.Attendance_Note,
                                            Attendance_Timestamp = a.Attendance_Timestamp,
                                            Attendance_MarkedBy = a.Attendance_MarkedBy,
                                            Attendance_Teacher = a.Teacher.Fullname,  // Lấy tên giáo viên
                                            Attendance_SubjectId = a.Attendance_SubjectId,
                                            Attendance_SubjectName = a.Attendance_SubjectName,
                                            StudentsId = a.StudentsId,
                                            Attendance_StudentName = a.Students.Fullname,
                                            Attendance_StudentCode = a.Students.Code
                                        })
                                    ).ToListAsync();

                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "200",
                    Message = "Get All Attendances by classId was successfully!",
                    Data = _mapper.Map<List<AttendanceResponse>>(attendances)
                };

            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public Task<ApiResponse<AttendanceResponse>> UpdateAttendance(AttendanceRequest request)
        {
            throw new NotImplementedException();
        }

        //[{},{}]
        public async Task<ApiResponse<AttendanceResponse>> CreateListAttendance(ListAttendanceRequest requests)
        {
            _logger.LogInformation("Cập nhật điểm danh");
            if (!DateTime.TryParse(requests.Attendance_Timestamp, out DateTime parsedDate))
            {
                return await Task.FromResult(new ApiResponse<AttendanceResponse>
                {
                    Code = "500",
                    Message = "Ngày không hợp lệ!",
                    Data = null
                });
            }
            string curUserId = _curentUserService.GetUserId();
            foreach (var item in requests.Attendance)
            {
                var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.Attendance_Timestamp.Date == parsedDate.Date && item.StudentsId == a.StudentsId); // nếu tồn tại thì cập nhật.
                if (attendance!=null) // tồn tại thì mình cập nhật
                {
                    attendance.Attendance_Note = item.Attendance_Note;
                    attendance.IsExcused = item.IsExcused;
                    attendance.IsUnexcused = item.IsUnexcused;
                    try
                    {
                        _context.Attendances.Update(attendance);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex) {
                        return new ApiResponse<AttendanceResponse>
                        {
                            Code = "500",
                            Message = ex.Message,
                            Data = null
                        };
                    }
                }
                else
                {
                    var newAttendance = new Attendance
                    {
                        StudentsId = item.StudentsId,
                        IsExcused = item.IsExcused,
                        IsUnexcused = item.IsUnexcused,
                        Attendance_Timestamp = parsedDate,
                        Attendance_Note = item.Attendance_Note,
                        Attendance_Semester = requests.Attendance_Semester,
                        Attendance_Year = requests.Attendance_Year,
                        Attendance_MarkedBy = curUserId,
                        Attendance_SubjectId = requests.Attendance_SubjectId,
                        Attendance_SubjectName = requests.Attendance_SubjectName,
                        TeacherId = curUserId
                    };
                    try
                    {
                        Console.WriteLine("TeacherId: ", newAttendance.Attendance_MarkedBy);
                        await _context.Attendances.AddAsync(newAttendance);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex) {
                        return new ApiResponse<AttendanceResponse>
                        {
                            Code = "500",
                            Message = ex.Message,
                            Data = null
                        };
                    }
                    
                }
            }

            return new ApiResponse<AttendanceResponse>
            {
                Code = "200",
                Message = "Create List Attendance was successfully!",
                Data = null
            };
        }

        public async Task<ApiResponse<AttendanceResponse>> DeleteAttendanceById(string id)
        {
            try
            {
                var attendance = await _context.Attendances.FirstOrDefaultAsync(a => a.Attendance_Id.ToString() == id);
                if (attendance == null)
                {
                    return new ApiResponse<AttendanceResponse>
                    {
                        Code = "404",
                        Message = "Attendance does not exist!",
                        Data = null
                    };
                }

                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                return new ApiResponse<AttendanceResponse>
                {
                    Code = "200",
                    Message = "Deleting attendance was successfully!",
                    Data = null
                };
            }
            catch (Exception ex) {
                return new ApiResponse<AttendanceResponse>
                {
                    Code = "500",
                    Message = "An error occured while deleting!",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<AttendanceResponse>>> GetAllAttendanceByStudentId(string studentId, string year)
        {
            try
            {
                var attendances = await _context.Attendances.Where(a => a.Attendance_Year == year && a.StudentsId == studentId)
                                            .Select(a => new AttendanceResponse
                                            {
                                                Attendance_Id = a.Attendance_Id,
                                                IsExcused = a.IsExcused,
                                                IsUnexcused = a.IsUnexcused,
                                                Attendance_Year = a.Attendance_Year,
                                                Attendance_Semester = a.Attendance_Semester,
                                                Attendance_Note = a.Attendance_Note,
                                                Attendance_Timestamp = a.Attendance_Timestamp,
                                                Attendance_MarkedBy = a.Attendance_MarkedBy,
                                                Attendance_Teacher = a.Teacher.Fullname,  // Lấy tên giáo viên
                                                Attendance_SubjectId = a.Attendance_SubjectId,
                                                Attendance_SubjectName = a.Attendance_SubjectName,
                                                StudentsId = a.StudentsId,
                                                Attendance_StudentName = a.Students.Fullname,
                                                Attendance_StudentCode = a.Students.Code
                                            }).ToListAsync();

                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "200",
                    Message = "Get All Attendances by classId was successfully!",
                    Data = _mapper.Map<List<AttendanceResponse>>(attendances)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AttendanceResponse>>
                {
                    Code = "500",
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<AbsenceStatisticsForTheMonthResponse>>> AbsenceStatisticsForTheMonth(Guid classId, string year)
        {
            var last28Days = DateTime.Today.AddDays(-28);

            var data = await _context.Attendances
                .Where(a => a.Students.ClassStudents.Any(cs => cs.Class_Id == classId && cs.Year == year)
                            && a.Attendance_Timestamp >= last28Days)
                .ToListAsync();

            var grouped = data
                .GroupBy(a => System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    a.Attendance_Timestamp.Date,
                    System.Globalization.CalendarWeekRule.FirstDay,
                    DayOfWeek.Monday))
                .Select(g => new {
                    Week = g.Key,
                    WithPermission = g.Count(x => x.IsExcused),
                    WithoutPermission = g.Count(x => x.IsUnexcused)
                })
                .OrderBy(x => x.Week)
                .ToList();
            Console.WriteLine("arr: " + string.Join(", ", grouped));
            var result = grouped.Select(g => new AbsenceStatisticsForTheMonthResponse
            {
                Week = g.Week,
                WithPermission = g.WithPermission,
                WithoutPermission = g.WithoutPermission
            }).ToList();

            return new ApiResponse<List<AbsenceStatisticsForTheMonthResponse>>
            {
                Code = "200",
                Message = "Successfully!",
                Data = result
            };
        }

        public async Task<ApiResponse<List<AbsentOfStudentResponse>>> GetAllAbsenceOfStudent(string studentId, List<string> listYear)
        {
            //listYear: ["2023-2024", "2022-2023", "2021-2022"]
            try
            {
                var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
                if(existingStudent == null)
                {
                    return new ApiResponse<List<AbsentOfStudentResponse>>
                    {
                        Code = "404",
                        Message = "Học sinh không tồn tại.",
                        Data = null
                    };
                }
                var result = new List<AbsentOfStudentResponse>();
                foreach (var year in listYear)
                {
                    var attendances = await _context.Attendances
                        .Where(a => a.StudentsId == studentId && a.Attendance_Year == year)
                        .Select(a => new AttendanceResponse
                            {
                                Attendance_Id = a.Attendance_Id,
                                IsExcused = a.IsExcused,
                                IsUnexcused = a.IsUnexcused,
                                Attendance_Year = a.Attendance_Year,
                                Attendance_Semester = a.Attendance_Semester,
                                Attendance_Note = a.Attendance_Note,
                                Attendance_Timestamp = a.Attendance_Timestamp,
                                Attendance_MarkedBy = a.Attendance_MarkedBy,
                                Attendance_Teacher = a.Teacher.Fullname,  // Lấy tên giáo viên
                                Attendance_SubjectId = a.Attendance_SubjectId,
                                Attendance_SubjectName = a.Attendance_SubjectName,
                                StudentsId = a.StudentsId,
                                Attendance_StudentName = a.Students.Fullname,
                                Attendance_StudentCode = a.Students.Code
                            }
                        )
                        .ToListAsync();
                    if(attendances != null)
                    {
                        var absentResponse = new AbsentOfStudentResponse
                        {
                            Year = year,
                            Absent = attendances
                        };
                        result.Add(absentResponse);
                    }
                }

                return new ApiResponse<List<AbsentOfStudentResponse>>
                {
                    Code = "200",
                    Message = "Lấy dữ liệu điểm danh của học sinh thành công.",
                    Data = result
                };

            }
            catch(Exception e)
            {
                return new ApiResponse<List<AbsentOfStudentResponse>>
                {
                    Code = "500",
                    Message = "Có lỗi khi lấy dữ liệu điểm danh của học sinh.",
                    Data = null
                };
            }
        }
    }
}
