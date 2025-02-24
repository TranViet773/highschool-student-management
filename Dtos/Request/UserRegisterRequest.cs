using NL_THUD.Models;
using NL_THUD.Models.Enum;

namespace NL_THUD.Dtos.Request
{
    public class UserRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }  
        public string Phone { get; set; }
        public DateOnly? DoB {  get; set; }
        public ERole ERole { get; set; }
        public Address? Address { get; set; }
    }
}
