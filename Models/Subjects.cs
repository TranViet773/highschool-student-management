using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Subjects
    {
        [Key]
        public Guid Subject_Id { get; set; } = Guid.NewGuid();
        public string Subject_Name { get; set; }
        public List<Teacher_Subject> Teacher_Subjects { get; set; } = new List<Teacher_Subject>();

        public List<Schedule_Detail> Schedule_Detail { get; set; } = new List<Schedule_Detail>();
        public List<Student_Score> student_Scores { get; set; } = new List<Student_Score>();

    }
}
