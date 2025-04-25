using NL_THUD.Models.Enum;
using NL_THUD.Models;

namespace NL_THUD.Dtos.Request
{
    public class UserUpdateRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateOnly? DoB { get; set; }

        //student
        public string? Student_Mother_Name { get; set; }
        public string? Student_Father_Name { get; set; }
        public string? Student_Mother_Career { get; set; }
        public string? Student_Father_Career { get; set; }
        public string? Student_Parent_Phone { get; set; }
    }
}
