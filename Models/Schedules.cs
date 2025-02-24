using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Schedules
    {
        [Key]
        public Guid Schedules_Id { get; set; } = Guid.NewGuid();
        public required string Schedules_Semester { get; set; }
        public List<Schedule_Detail> Schedules_Detail { get; set;} = new List<Schedule_Detail>();
        public List<Class_Student> Class_Students { get; set; } = new List<Class_Student>();
    }
}
