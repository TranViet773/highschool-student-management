namespace NL_THUD.Dtos.Response
{
    public class StatisticalGenerateResponse
    {
        public int NumberOfStudent { get; set; }
        public int NumberOfTeacher { get; set; }
        public int NumberOfStaff { get; set; }
        public int NumberOfClass { get; set; }
        public int NumberOfSubject { get; set; }
        public int NumberOfReevaluation { get; set; }
        public List<NumberOfStudentByGender> NumberOfStudentByGender { get; set; }
        public List<NumberOfStudentByGrade> NumberOfStudentByGrade { get; set; }
        public List<NumberOfReevaluationByYear> NumberOfReevaluationByYear { get; set; }
        public List<NumberOfStudentEnrolled> NumberOfStudentEnrolleds { get; set; }
    }

    public class NumberOfStudentByGrade
    {
        public string Year { get; set; }
        public int NumberOfStudentOf10 { get; set; }
        public int NumberOfStudentOf11 { get; set; }
        public int NumberOfStudentOf12 { get; set; }
    }

    public class NumberOfReevaluationByYear
    {
        public string Year { get; set; }
        public int Handled { get; set; }
        public int NotHandleYet { get; set; }
    }

    public class NumberOfStudentByGender
    {
        public int Male { get; set; }
        public int Female { get; set; } 
        public int Gender { get; set; }
        public string Year { get; set; }
    }

    public class NumberOfStudentEnrolled
    {
        public string Year { get; set; }
        public int enrolledStudents { get; set; }
    }

}
