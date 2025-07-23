namespace NL_THUD.Dtos.Response
{
    public class ClassOfStudentResponse
    {
        public List<ClassOfStudent> ClassOfStudents { get; set; }
    }

    public class ClassOfStudent
    {
        public string ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public string Year { get; set; }
    }
}
