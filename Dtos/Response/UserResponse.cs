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
        public string PhoneNumber { get; set; }
        public bool? isDeleted { get; set; }
        public bool? isBlocked { get; set; }
        public bool? isActive { get; set; }
        public DateTime? createAccountAt { get; set; }
        public DateTime? latestLogIn { get; set; }
        public bool Gender { get; set; }
        public DateTime CreateAt { get; set; } //RefreshToken
        public DateTime UpdateAt { get; set; }
        public string? RefreshToken { get; set; }
        public string? AccessToken { get; set; }
        public string? ClassCode { get; set; }
        public string? ClassId { get; set; }

        //Student
        public string? Student_Mother_Name { get; set; }
        public string? Student_Father_Name { get; set; }
        public string? Student_Mother_Career { get; set; }
        public string? Student_Father_Career { get; set; }
        public string? Student_Parent_Phone { get; set; }
        public AddressResponse? Address { get; set; }
    }
}
