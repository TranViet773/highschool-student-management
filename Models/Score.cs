using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Score
    {
        [Key]
        public Guid Score_Id { get; set; } = Guid.NewGuid();
        public float Score_1{ get; set; }
        public float Score_2 { get; set; }
        public float Score_3 { get; set; }
        public float Score_final { get; set; }
        public List<Student_Score> student_Scores { get; set; } = new List<Student_Score>();
    }
}
