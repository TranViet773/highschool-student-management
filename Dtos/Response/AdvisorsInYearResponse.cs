namespace NL_THUD.Dtos.Response
{
    public class AdvisorsInYearResponse
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string year { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public Guid Class_Id { get; set; }
    }
}
