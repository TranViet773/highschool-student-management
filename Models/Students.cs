using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Students : Person
    {
        public SystemAdmin SystemAdmin { get; set; }
        [ForeignKey("SystemAdmin_Id")]
        public string SystemAdminId { get; set; }
        public Parents Parents { get; set; }
        [ForeignKey("ParentId")]
        public string ParentId { get; set; }
        public string? Student_Mother_Name { get; set; }
        public string? Student_Father_Name { get; set; }
        public string? Student_Mother_Career { get; set; }
        public string? Student_Father_Career { get; set; }
        public string? Student_Parent_Phone { get; set; }

        public List<Class_Student> ClassStudents { get; set; } = new List<Class_Student>();
        public List<Student_Score> student_Scores { get; set; } = new List<Student_Score>();
        public List<Evaluation> Evaluations { get; set; }
    }
}
