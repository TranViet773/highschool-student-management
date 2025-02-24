namespace NL_THUD.Dtos.Response
{
    public class CurrentUserResponse
    {
        public string LastName { get; set; }
        public string FirstName {  get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
