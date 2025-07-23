using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Attendance
    {
        [Key]
        public Guid Attendance_Id { get; private set; } = Guid.NewGuid();
        public bool IsExcused { get; set; } = false;
        public bool IsUnexcused { get; set; } = false;
        public string Attendance_Year { get; set; }
        public int Attendance_Semester { get; set; }
        public string Attendance_Note { get; set; }
        public DateTime Attendance_Timestamp { get; set; }
        public string Attendance_MarkedBy { get; set; }
        public string? Attendance_SubjectName { get; set; }
        public string?   Attendance_SubjectId { get; set; }

        // Quan hệ với học sinh(Student)
        public string StudentsId { get; set; }
        public Students Students { get; set; }
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }  // <== Điều hướng đến Teacher

    }
}
