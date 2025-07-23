using NL_THUD.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Dtos.Response
{
    public class AttendanceResponse
    {
        public Guid Attendance_Id { get; set; }
        public bool IsExcused { get; set; } = false;
        public bool IsUnexcused { get; set; } = false;
        public string Attendance_Year { get; set; }
        public int Attendance_Semester { get; set; }
        public string Attendance_Note { get; set; }
        public DateTime Attendance_Timestamp { get; set; }
        public string Attendance_MarkedBy { get; set; }
        public string Attendance_Teacher { get; set; }
        public string Attendance_SubjectId { get; set; }
        public string Attendance_SubjectName { get; set; }
        public string StudentsId { get; set; }
        public string Attendance_StudentName { get; set; }
        public string Attendance_StudentCode { get; set; }
        public Teacher Teacher { get; set; }
    }
}
