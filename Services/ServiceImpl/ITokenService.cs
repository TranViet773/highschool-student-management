using NL_THUD.Models;

namespace NL_THUD.Services.ServiceImpl
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Person person);
        string GenerateRefreshToken();
    }
}
