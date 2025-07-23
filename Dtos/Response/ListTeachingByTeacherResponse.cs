namespace NL_THUD.Dtos.Response
{
    public class ListTeachingByTeacherResponse
    {
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherCode { get; set; }
        public List<TeachingByTeacherResponse> TeachingByTeacher { get; set; }
    }
    public class TeachingByTeacherResponse
    {
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public string ClassId { get; set; }
    }
}
