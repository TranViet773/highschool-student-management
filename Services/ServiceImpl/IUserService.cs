using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;

namespace NL_THUD.Services.ServiceImpl
{
    public interface IUserService
    {
        Task<UserResponse> RegisterAsync(UserRegisterRequest request);
        Task<UserResponse> LogInAsync(UserLoginRequest request);
        Task<UserResponse> GetUserByIdAsync(Guid id, string year);
        Task<CurrentUserResponse> GetCurrentUserAsync();
        Task<UserResponse> UpdateUserAsync(UserUpdateRequest register, Guid userId);
        Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest refreshToken);
        Task<CurrentUserResponse> RefreshToken(RefreshTokenRequest refreshToken);
        Task<ApiResponse<string>> DeleteUserById(Guid id);   
        Task<List<UserResponse>> GetAllUsersAsync(string role);
        Task<List<UserResponse>> filterUser(string? codeClass, string? year, string? query, string role);
        Task<ApiResponse<UserResponse>> changePasswordAsync(UserChangePasswordRequest request, Guid id);
        Task<ApiResponse<UserResponse>> getAdvisorByStudent(string classId, string year, string semester);

        Task<ApiResponse<UserResponse>> BlockAndUnblockUser(string userId);
        Task<ApiResponse<UserResponse>> SetActiveUser(string userId);
    }
}
