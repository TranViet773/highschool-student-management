namespace NL_THUD.Dtos.Request
{
    public class AttendanceRequest
    {
        public bool IsExcused { get; set; }
        public bool IsUnexcused { get; set; }
        public string? Attendance_Year { get; set; }
        public int? Attendance_Semester { get; set; }
        public string Attendance_Note { get; set; }
        public string StudentsId { get; set; }
        public string? Attendance_Timestamp { get; set; }
        public string? Attendance_SubjectName { get; set; }
        public string? Atendance_SubjectId { get; set; }
    }
}
