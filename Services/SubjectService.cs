using AutoMapper;
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
