using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IStudentService
    {
        Task<List<UserResponse>> filterUser(string? codeClass, string? year, string? query, string role);
    }
}
