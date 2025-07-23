using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NL_THUD.Models
{
    public class Student_Score
    {

        public int Id { get; set; }

        // Điểm chi tiết
        public double? OralScore { get; set; }  // Điểm miệng
        public double? OralScore1 { get; set; }  // Điểm miệng
        public double? OralScore2 { get; set; }  // Điểm miệng
        public double? OralScore3 { get; set; }  // Điểm miệng
        public double? QuizScore { get; set; }  // Điểm 15 phút
        public double? QuizScore1 { get; set; }  // Điểm 15 phút
        public double? QuizScore2 { get; set; }  // Điểm 15 phút
        public double? QuizScore3 { get; set; }  // Điểm 15 phút
        public double? TestScore { get; set; }  // Điểm 1 tiết
        public double? MidTearmScore { get; set; }
        public double? FinalExamScore { get; set; }  // Điểm thi cuối kỳ
        public DateTime? OralScore_Timestamp { get; set; }
        public DateTime? OralScore1_Timestamp { get; set; }
        public DateTime? OralScore2_Timestamp { get; set; }
        public DateTime? OralScore3_Timestamp { get; set; }
        public DateTime? QuizScore_Timestamp { get; set; }
        public DateTime? QuizScore1_Timestamp { get; set; }
        public DateTime? QuizScore2_Timestamp { get; set; }
        public DateTime? QuizScore3_Timestamp { get; set; }
        public DateTime? TestScore_Timestamp { get; set; }
        public DateTime? MidTermScore_Timestamp { get; set; }
        public DateTime? FinalExamScore_Timestamp { get; set; }

        //Phân biệt là điểm hây Đ,CĐ
        public bool ScoringType { get; set; } = true; //TRUE: Đ,KD - FALSE: Điểm

        // Nhận xét của Giáo viên giảng dạy trong một học kỳ
        public string? Comment { get; set; }
        public DateTime? Comment_Expire { get; set; }
        public DateTime? Comment_UpdateAt { get; set; }


        public string Year { get; set; }
        public int Semester { get; set; }

        // Điểm trung bình môn cho học kỳ
        public double? AverageScore { get; set; }

        public string StudentId { get; set; }
        public Students Students { get; set; }

        public Guid SubjectId { get; set; }

        [JsonIgnore]
        public Subjects Subjects { get; set; }
    }
}
