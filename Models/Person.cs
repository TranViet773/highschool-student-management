using Microsoft.AspNetCore.Identity;
using NL_THUD.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Person : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Code { get; set; } // MÃ số cán bộ, giáo viên, nhân viên, học sinh. VD: HS00001, GV00002
        public string? Avatar {  get; set; }
        public string? Fullname {  get; set; }
        public DateOnly DoB {  get; set; }
        public bool Gender {  get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public required ERole ERole { get; set; }  
        public List<Address>? Addresses { get; set; } = new List<Address>();
    }
}
