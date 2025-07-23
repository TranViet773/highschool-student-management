namespace NL_THUD.Dtos.Response
{
    public class ScoreBoardOfClassInSemesterResponse
    {
        public string? ClassName { get; set; }
        public int Semester { get; set; }
        public string SubjectName { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string StudentCode { get; set; }
        public ScoreBoardResponse ScoresOfStudent { get; set; }
        public ResultFinalYear resultFinalYear { get; set; }
    }
}
