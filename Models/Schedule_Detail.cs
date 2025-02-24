using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Schedule_Detail
    {
        public int PeriodNumber { get; set; }
        public int Session {  get; set; }
        public int DayOfWeek { get; set; }
        
        public Schedules Schedules { get; set; }
        [ForeignKey("ScheduleId")]
        public Guid ScheduleId { get; set; }

        public Subjects Subjects { get; set; }
        [ForeignKey("SubjectId")]
        public Guid SubjectId { get; set; }
    }
}
