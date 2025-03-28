using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IStudentService
    {
        Task<List<UserResponse>> getAllByClass(string codeClass);
        Task UpdateStudent(Guid id);
    }
}
