using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Student_Score
    {

        public int Id { get; set; }

        // Điểm chi tiết
        public double? OralScore { get; set; }  // Điểm miệng
        public double? QuizScore { get; set; }  // Điểm 15 phút
        public double? TestScore { get; set; }  // Điểm 1 tiết
        public double? MidTearmScore { get; set; }
        public double? FinalExamScore { get; set; }  // Điểm thi cuối kỳ
        public DateTime? OralScore_Timestamp { get; set; }
        public DateTime? QuizScore_Timestamp { get; set; }
        public DateTime? TestScore_Timestamp { get; set; }
        public DateTime? MidTermScore_Timestamp { get; set; }
        public DateTime? FinalExamScore_Timestamp { get; set; }
        public DateTime? OralScore_Expire { get; set; }
        public DateTime? QuizScore_Expire { get; set; }
        public DateTime? TestScore_Expire { get; set; }
        public DateTime? MidTermScore_Expire { get; set; }
        public DateTime? FinalExamScore_Expire { get; set; }

        public string? Comment { get; set; }
        public DateTime? Comment_Expire { get; set; }
        public DateTime? Comment_UpdateAt { get; set; }
        public string Year { get; set; }
        public int Semester { get; set; }

        // Điểm trung bình môn cho học kỳ
        public double? AverageScore => (OralScore + QuizScore + (2 * TestScore) + (2* MidTearmScore) + (3 * FinalExamScore)) / 9;

        public string StudentId { get; set; }
        public Students Students { get; set; }

        public Guid SubjectId { get; set; }
        public Subjects Subjects { get; set; }
    }
}
