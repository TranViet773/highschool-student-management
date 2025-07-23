using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NL_THUD.Models
{
    public class Subjects
    {
        [Key]
        public Guid Subject_Id { get; set; } = Guid.NewGuid();
        public string Subject_Name { get; set; }

        // Thêm một trường Điểm được đánh giá bằng điểm hay bằng nhận xét.
        public bool Subject_ScoringType { get; set; } = true; //  TRUE: Đ, KĐ - FALSE: Điểm

        public List<Teacher_Subject> Teacher_Subjects { get; set; } = new List<Teacher_Subject>();

        public List<Schedule_Detail> Schedule_Detail { get; set; } = new List<Schedule_Detail>();

        [JsonIgnore]
        public List<Student_Score> student_Scores { get; set; } = new List<Student_Score>();
        public List<Reevaluation> Reevaluations { get; set; }
    }
}
