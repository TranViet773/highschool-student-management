namespace NL_THUD.Dtos.Response
{
    public class ListTeacherBySubjectsResponse
    {
        public string year { get; set; }
        public int semester {  get; set; }      
        public List<TeacherSubjectDto> TeacherSubjects {  get; set; }
    }

    public class TeacherSubjectDto
    {
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }
    }
}
