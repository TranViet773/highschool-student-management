namespace NL_THUD.Dtos.Request
{
    public class UserChangePasswordRequest
    {
        public string NewPassword { get; set; } 
        public string OldPassword { get; set; }
    }
}
