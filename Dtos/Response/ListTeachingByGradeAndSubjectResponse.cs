namespace NL_THUD.Dtos.Response
{
    public class ListTeachingByGradeAndSubjectResponse
    {
        public string Year { get; set; }
        public int Grade { get; set; }
        public List<TeachingByGradeAndSubjectResponse> TeachingByGradeAndSubjectResponse { get; set; }
    }

    public class TeachingByGradeAndSubjectResponse
       {
        public string ClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string TeacherCode { get; set; }
        public string TeacherId { get; set; }
        public string TeacherName { get; set; }    
    }
}
