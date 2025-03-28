using NL_THUD.Models.Enum;
using NL_THUD.Models;

namespace NL_THUD.Dtos.Request
{
    public class UserUpdateRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public DateOnly? DoB { get; set; }
    }
}
