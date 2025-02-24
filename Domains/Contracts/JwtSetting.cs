namespace NL_THUD.Domains.Contracts
{
    public class JwtSetting
    {
        public string SecretKey {  get; set; }  
        public string ValidIssuer { get; set; }
        public string ValidAudience {  get; set; }
        public string Expires {  get; set; }
    }
}
