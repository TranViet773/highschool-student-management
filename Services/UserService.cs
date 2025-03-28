using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NL_THUD.Data;
using NL_THUD.Dtos.Request;
using NL_THUD.Dtos.Response;
using NL_THUD.Models;
using NL_THUD.Models.Enum;
using NL_THUD.Services.ServiceImpl;
using System.Globalization;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NL_THUD.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService tokenService;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public UserService(ITokenService tokenService, ILogger<UserService> logger, ICurrentUserService currentUserService, UserManager<Person> userManager, IMapper mapper, ApplicationDbContext context, IAddressService addressService)
        {
            this.tokenService = tokenService;
            _logger = logger;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
            _addressService = addressService;
            _context = context;
        }

        public async Task<ApiResponse<string>> DeleteUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) {
                _logger.LogError("User is not found!");
                return new ApiResponse<string>
                {
                    Code = "404",
                    Message = "User is not found!!",
                    Data = ""
                };
            }
            await _userManager.DeleteAsync(user);
            return new ApiResponse<string>
            {
                Code = "200",
                Message = "Successfully!",
                Data = ""
            };
        }

        public async Task<CurrentUserResponse> GetCurrentUserAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId());
            if (user == null)
            {
                _logger.LogError("User not found!");
                throw new Exception("User not found!");
            }
            return _mapper.Map<CurrentUserResponse>(user);
        }

        public async Task<UserResponse> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogError("User not found!");
                throw new Exception("User not found!");
            }
            var year = $"{DateTime.Now.Year}-{DateTime.Now.Year + 1}";

            var result = _mapper.Map<UserResponse>(user);
            Guid userId = Guid.Parse(user.Id);
            var data = await _addressService.GetAddressAsync(userId);
            if(data.Data != null)
            {
                result.Address = data.Data;
            }
            var ClassStudent = _context.ClassStudents.FirstOrDefault(u => u.Student_Id == result.Id.ToString() && u.Year == year);
            if (ClassStudent != null)
            {
                var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == ClassStudent.Class_Id);
                result.ClassCode = Class.Classes_Code;
            }
            return result;
        }

        public async Task<UserResponse> LogInAsync(UserLoginRequest request)
        {
            //Find user
            var user = await _userManager.FindByEmailAsync(request.Email);
            bool isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (user == null || !isValidPassword)
            {
                _logger.LogError("User or password is wrong!");
                throw new InvalidOperationException("User or password is wrong!");
            }
            //Check password
            //Generate Token and RefreshToken
            var token = await tokenService.GenerateToken(user);
            var refreshToken =  tokenService.GenerateRefreshToken(); // base64
            //Hash RefreshToken
            using var sha256 = SHA256.Create();
            var refreshTokenHashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken)); //byte[] 32
            user.RefreshToken = Convert.ToBase64String(refreshTokenHashed); // base64
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(1);
            //update Token and Refreshtoken 
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) {

                _logger.LogError("Failed to update user!");
                throw new Exception("Failed to update user!");
            }
            //mapping and return userResponse
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.RefreshToken = refreshToken;
            userResponse.AccessToken = token;
            userResponse.UpdateAt = DateTime.Now;
            userResponse.CreateAt = DateTime.Now;
            if(user.initialPasswordExpiry > DateTime.Now.AddHours(12) && user.isActive == false) // check xem cái pass 1st còn hiệu lực không, nếu không thì khóa.
            {
                userResponse.isBlocked = true;
            }
            return userResponse;
        }

        public async Task<CurrentUserResponse> RefreshToken(RefreshTokenRequest request)
        {
            _logger.LogInformation("RefreshToken");
            using var sha256 = SHA256.Create();
            var refreshTokenHashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
            var refreshToken = Convert.ToBase64String(refreshTokenHashed);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                _logger.LogError("Do not found user has RefreshToken!");
                throw new Exception("Do not found user has RefreshToken!"); 
            }
            if (user.RefreshTokenExpireTime < DateTime.Now)
            {
                _logger.LogError("RefreshToken has expired!");
                throw new Exception("RefreshToken has expired!");
            }
            var accessToken = await tokenService.GenerateToken(user);
            _logger.LogInformation("Generate new AccessToken successfully!");
            var currUserResponse = _mapper.Map<CurrentUserResponse>(user);
            currUserResponse.AccessToken = accessToken; // Client nhận được accesstoken mới này nè
            return currUserResponse;
        }

        public async Task<UserResponse> RegisterAsync(UserRegisterRequest request)
        {
            _logger.LogInformation("Register new user!");

            // Kiểm tra email đã tồn tại chưa
            var isExistedUser = await _userManager.FindByEmailAsync(request.Email);
            if (isExistedUser != null)
            {
                _logger.LogError("User already exists!");
                throw new Exception("User already exists!");
            }

            // Xác định loại người dùng (Teacher hoặc Student)
            Person newUser;
            switch (request.ERole.ToString())
            {
                case "TEACHER": newUser = _mapper.Map<Teacher>(request); break;
                case "STUDENT": newUser = _mapper.Map<Students>(request); break;
                case "PARENT": newUser = _mapper.Map<Parents>(request); break;
                case "SYS_ADMIN": newUser = _mapper.Map<SystemAdmin>(request); break;
                case "MANAGERMENT_STAFF": newUser = _mapper.Map<ManagementStaff>(request); break;
                default: throw new Exception("Invalid role!");
            }
           

            // Gán các thuộc tính chung
            newUser.UserName = GenerateUsername(request.FirstName, request.LastName);
            newUser.Code = await GenerateCode(newUser);
            newUser.Fullname = $"{request.LastName} {request.FirstName}";
            newUser.initialPasswordExpiry = DateTime.Now.AddHours(12); // này là thời gian đổi pass word lần đầu tiên.
            // Tạo user trong database  
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user: {errors}", errors);
                throw new Exception($"Failed to create user: {errors}");
            }

            _logger.LogInformation($"Registered user: {request.Email}");

            // Tạo token JWT
            var token = await tokenService.GenerateToken(newUser);

            return _mapper.Map<UserResponse>(newUser);
        }


        public async Task<string> GenerateCode(Person user)
        {
            var year = DateTime.Now.Year.ToString();
            var preYear = year.Substring(2, 2); // Lấy "24" thay vì "2024"

            // Xác định tiền tố theo vai trò
            string prefix = user.ERole switch
            {
                ERole.STUDENT => "HS",
                ERole.PARENT => "PH",
                ERole.TEACHER => "GV",
                ERole.MANAGERMENT_STAFF => "NV",
                ERole.SYS_ADMIN => "SA",
                _ => throw new ArgumentException("Vai trò không hợp lệ")
            };

            // Lấy mã cao nhất hiện có
            var currentCode = await _userManager.Users
                .Where(u => u.ERole == user.ERole)
                .MaxAsync(x => x.Code);

            if (string.IsNullOrEmpty(currentCode))
            {
                return $"{prefix}{preYear}0001"; // Mã đầu tiên
            }

            // Tách số từ mã hiện có
            Match match = Regex.Match(currentCode, @"(\D+)(\d+)");
            if (match.Success)
            {
                string oldPrefix = match.Groups[1].Value; // "HS24"
                int number = int.Parse(match.Groups[2].Value); // 0001

                number++; // Tăng số lên 1
                return $"{oldPrefix}{number:D4}"; // Định dạng lại số: 0002 -> 0003
            }

            throw new InvalidOperationException("Mã hiện tại không đúng định dạng");
        }

        public string GenerateUsername(string FirstName, string LastName)
        {
            // Loại bỏ dấu tiếng Việt
            string RemoveVietnameseTones(string text)
            {
                text = text.Normalize(NormalizationForm.FormD);
                return new string(text
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray())
                    .Normalize(NormalizationForm.FormC)
                    .Replace("đ", "d").Replace("Đ", "D");
            }

            var baseName = RemoveVietnameseTones($"{FirstName}{LastName}").ToLower();
            baseName = Regex.Replace(baseName, @"[^a-z0-9]", ""); // Chỉ giữ chữ và số

            var username = baseName;
            int count = 1;

            // Kiểm tra username trùng và tăng số thứ tự
            while (_userManager.Users.Any(u => u.UserName == username))
            {
                username = $"{baseName}{count}";
                count++;
            }

            return username;
        }


        public async Task<RevokeRefreshTokenResponse> RevokeRefreshToken(RefreshTokenRequest request)
        {
            _logger.LogInformation("Revoke Refresh Token");
            try
            {
                using var sha256 = SHA256.Create();
                var refreshTokenHashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
                var refreshToken = Convert.ToBase64String(refreshTokenHashed);
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
                if (user == null) {
                    _logger.LogError("Do not found user has refreshToken!");
                    throw new Exception("Do not found user has refreshToken!");
                }
                //Check expired refreshtoken
                if (user.RefreshTokenExpireTime < DateTime.Now)
                {
                    _logger.LogError("RefreshToken has expired!");
                    throw new Exception("Refresh Token has expired!");
                }
                //Set null for expireTime
                user.RefreshTokenExpireTime = null;
                user.RefreshToken = null;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) {
                    _logger.LogError("Failed to revoke refresh token!");
                    throw new Exception("Failed to revoke refresh token!");
                }
                _logger.LogInformation("Refresh token revoked successfully!");
                return new RevokeRefreshTokenResponse
                {
                    Message = "Refresh token revoked successfully!"
                };
            } catch (Exception ex)
            {
                _logger.LogError($"Failed to revoke refresh token: {ex.Message}");
                throw new Exception("Failed to revoke refresh token!");
            }
        }

        public async Task<UserResponse> UpdateUserAsync(UserUpdateRequest request, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogError($"User with ID {userId} not found");
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            // Chỉ cập nhật những giá trị từ request mà khác null
            _mapper.Map(request, user);
            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                int spaceIndex = request.FullName.IndexOf(' '); // Tìm vị trí khoảng trắng đầu tiên

                if (spaceIndex != -1) // Nếu có khoảng trắng
                {
                    user.FirstName = request.FullName.Substring(0, spaceIndex); // Lấy phần trước khoảng trắng
                    user.LastName = request.FullName.Substring(spaceIndex + 1); // Lấy phần sau khoảng trắng
                }
                else
                {
                    user.FirstName = request.FullName; // Nếu không có khoảng trắng, lấy toàn bộ làm FirstName
                    user.LastName = ""; // Không có LastName
                }
            }

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError($"Failed to update user {userId}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new Exception("Failed to update user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update user {userId}: {ex.Message}");
                throw new Exception("Failed to update user", ex);
            }

            _logger.LogInformation($"Updated user {userId} successfully!");
            return _mapper.Map<UserResponse>(user);
        }


        public async Task<List<UserResponse>> GetAllUsersAsync(string role)
        {
            if (!Enum.TryParse(role, true, out ERole roleEnum)) // Chuyển chuỗi thành enum (không phân biệt hoa/thường)
            {
                throw new ArgumentException("Invalid role value"); // Xử lý lỗi nếu role không hợp lệ
            }

            var users = await _userManager.Users
                .Where(u => u.ERole == roleEnum) // So sánh enum trực tiếp
                .ToListAsync();
            
            return _mapper.Map<List<UserResponse>>(users);
        }

        public async Task<List<UserResponse>> filterUser(string? codeClass, string? year, string? query, string role)
        {
            var eRole = ERole.STUDENT;
            if (string.IsNullOrEmpty(year))
            {
                year = DateTime.Now.Year.ToString();
            }
            var preYear = year.Substring(2, 2);
            string tmp = "";
            switch (role)
            {
                case "STUDENT": eRole = ERole.STUDENT; tmp = $"HS{preYear}"; break;
                case "TEACHER": eRole = ERole.TEACHER; tmp = $"GV{preYear}"; break;
                case "MANAGEMENT_STAFF": eRole = ERole.MANAGERMENT_STAFF; tmp = $"NV{preYear}"; break;
            }

            var isNumber = query != null && query.Length >= 4 && char.IsDigit(query[2]) && char.IsDigit(query[3]);

            var result = await _userManager.Users
                .Where(u => u.ERole == eRole)
                .Where(u => u.Code.Length >= 4 && u.Code.Substring(0, 4) == tmp)
                .ToListAsync();

            if (!string.IsNullOrEmpty(query))
            {
                result = result.Where(u => isNumber ? u.Code == query : u.Fullname.Contains(query)).ToList();
            }

            
            if (!string.IsNullOrEmpty(codeClass))
            {
                var classEntity = await _context.Classes.FirstOrDefaultAsync(u => u.Classes_Code == codeClass);
                result = await _context.Students.Where(s => s.ClassStudents.Any(cs => cs.Class_Id == classEntity.Classes_Id)).Cast<Person>().ToListAsync();
               
            }

            var mappedResult = _mapper.Map<List<UserResponse>>(result);
            foreach (var user in mappedResult)
            {
                var classStudent = await _context.ClassStudents.FirstOrDefaultAsync(u => u.Student_Id == user.Id.ToString() && u.Year == year);
                if (classStudent != null)
                {
                    var Class = await _context.Classes.FirstOrDefaultAsync(c => c.Classes_Id == classStudent.Class_Id);
                    if (Class != null)
                    {
                        user.ClassCode = Class.Classes_Code;
                    }
                }
            }
            return mappedResult;
        }

        public async Task<ApiResponse<UserResponse>> changePasswordAsync(UserChangePasswordRequest request, Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user is null)
            {
                _logger.LogError("User do not found!");
                return new ApiResponse<UserResponse>
                {
                    Code = "404",
                    Message = "User do not found!",
                    Data = null
                };
            }

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, request.OldPassword); // true: match - false: do not match
            if (!checkOldPassword)
            {
                return new ApiResponse<UserResponse>
                {
                    Code = "400",
                    Message = "Old password is incorrect!",
                    Data = null
                };
            }
            var changePassword = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!changePassword.Succeeded)
            {
                return new ApiResponse<UserResponse>
                {
                    Code = "400",
                    Message = "Failed to change password!",
                    Data = null
                };
            }
            else
            {
                if (user.isActive == false) {
                    user.isActive = true;
                    await _userManager.UpdateAsync(user);
                }
                return new ApiResponse<UserResponse>
                {
                    Code = "200",
                    Message = "Password changed successfully!",
                    Data = null
                };

            }
        }
    }
}
