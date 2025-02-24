using Azure.Core;

namespace NL_THUD.Dtos.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Code {  get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email {   get; set;  }
        public string Username { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? ClassCode { get; set; }
    }
}
