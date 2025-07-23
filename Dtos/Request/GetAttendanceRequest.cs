namespace NL_THUD.Dtos.Request
{
    public class GetAttendanceRequest
    {
        public string Attendance_Year { get; set; }
        public int Attendance_Semester { get; set; }
        public string? Student_Id { get; set; }
        public Guid Attendance_ClassId { get; set; }  
    }
}
