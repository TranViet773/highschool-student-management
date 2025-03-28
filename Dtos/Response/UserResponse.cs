using Azure.Core;
using NL_THUD.Models;

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
        public string Avatar { get; set; }
        public DateOnly DoB { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isBlocked { get; set; }
        public bool? isActive { get; set; }
        public bool Gender { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? ClassCode { get; set; }
        public AddressResponse? Address { get; set; }
    }
}
