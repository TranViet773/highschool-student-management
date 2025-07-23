namespace NL_THUD.Dtos.Response
{
    public class FinalResultOfClassResponse
    {
        public string ClassName { get; set; }
        public string ClassId { get; set; }
        public string Year { get; set; }
        public int Semester { get; set; }
        public int QuantityOfStudent { get; set; }

        public List<StudentFinalResultDto> StudentData { get; set; }

        public FinalClassSummaryDto FinalResult { get; set; }
    }

    public class StudentFinalResultDto
    {
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public string StudentId { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public double? AVG_Semester_Score { get; set; }
        public string Conduct { get; set; }
        public string Classify { get; set; }
        public string Performance { get; set; }
        public int Rank { get; set; }
        public int QuantityOfAbsent { get; set; }
        public int QuantityOfAbsentWithPermission { get; set; }
        public int QuantityOfAbsentWithoutPermission { get; set; }
        public ScoreBoardResponse scoreBoard { get; set; } // Để hiển thị điểm thi các môn
        public string? Khenthuong { get; set; }
        public bool? isPassClass { get; set; }
    }

    public class FinalClassSummaryDto
    {
        public double AverageScore { get; set; }
        public List<SummaryItem> Conduct { get; set; }
        public List<SummaryItem> Classify { get; set; }
        public List<SummaryItem> Performance { get; set; }
        public List<SummaryItem> Khenthuong { get; set; }
        public List<SummaryItem> Lenlop { get; set; }
    }

    public class SummaryItem
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
