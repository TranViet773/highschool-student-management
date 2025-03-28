namespace NL_THUD.Dtos.Response
{
    public class ClassSubjectResponse
    {
        public Guid Classes_Id { get; set; }
        public string Classes_Name { get; set; }
        public string Classes_Code { get; set; }
        public string Classes_Quantity { get; set; }
        public string Subject_Name { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
        public bool? Classes_IdDeleted { get; set; }
        public string? advisor { get; set; }
        public List<UserResponse>? Students { get; set; }
    }
}
