using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Classes
    {
        [Key]
        public Guid Classes_Id { get; set; } = Guid.NewGuid();
        public string Classes_Name { get; set; }
        public string Classes_Quantity { get; set; }
        public List<Teacher_Class> TeacherClass { get; set; } = new List<Teacher_Class>();
        public List<Class_Student> Class_Student { get; set; } = new List<Class_Student>();
        public List<Teacher_Subject> TeacherSubject { get; set; } = new List<Teacher_Subject>();

    }
}
