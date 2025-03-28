using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NL_THUD.Data;
using NL_THUD.Dtos.Response;
using NL_THUD.Services.ServiceImpl;

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

        public Task UpdateStudent(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
