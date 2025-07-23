namespace NL_THUD.Dtos.Request
{
    public class ListAttendanceRequest
    {
        public string Attendance_Year { get; set; }
        public int Attendance_Semester { get; set; }
        public string Attendance_Timestamp { get; set; }
        public string? Attendance_SubjectName { get; set; }
        public string? Attendance_SubjectId { get; set; }
        public List<AttendanceRequest> Attendance { get; set; }
    }
}
