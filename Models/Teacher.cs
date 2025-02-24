using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Teacher : Person
    {
        public SystemAdmin SystemAdmin { get; set; }
        [ForeignKey("SystemAdmin_Id")]
        public string SystemAdminId { get; set; }

        public List<Teacher_Class> TeacherClass { get; set; } = new List<Teacher_Class>();
        public List<Teacher_Subject> TeacherSubject { get; set; } = new List<Teacher_Subject>();
        public List<AcademicTranscript> AcademicTranscript { get; set; } = new List<AcademicTranscript>();
    }
}
