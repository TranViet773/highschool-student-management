using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Class_Student
    {
        [ForeignKey("Class_Id")]
        public Guid Class_Id { get; set; }
        public Classes Class { get; set; }

        [ForeignKey("Student_Id")]
        public string Student_Id {  get; set; }
        public Students Students { get; set; }

        [ForeignKey("Schedule_Id")]
        public Guid Schedule_Id { get; set; }
        public Schedules schedules { get; set; }

        public string Year { get; set; }
    }
}
