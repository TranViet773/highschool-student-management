using NL_THUD.Models;

namespace NL_THUD.Dtos.Request
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string DoB { get; set; }
        public Address Address { get; set; }
        public string Avatar { get; set; }
    }
}
