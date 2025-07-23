namespace NL_THUD.Dtos.Response
{
    public class AbsentOfStudentResponse
    {
        public string Year { get; set; }
        public List<AttendanceResponse> Absent { get; set; }
    }
}
