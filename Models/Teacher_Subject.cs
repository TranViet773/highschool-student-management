using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Teacher_Subject
    {
        [ForeignKey("Subject_Id")]
        public Guid Subject_Id { get; set; }
        public Subjects Subjects { get; set; }

        [ForeignKey("Teacher_Id")]
        public string Teacher_Id { get; set; }
        public Teacher Teacher { get; set; }

        [ForeignKey("Class_Id")]
        public Guid Class_Id { get; set; }
        public Classes Classes { get; set; }

        public string Year { get; set; }
        public string Semester { get; set; }


    }
}
