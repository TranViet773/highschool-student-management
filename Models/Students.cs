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

        public List<Class_Student> Class_Students { get; set; } = new List<Class_Student>();
        public List<Student_Score> student_Scores { get; set; } = new List<Student_Score>();

    }
}
