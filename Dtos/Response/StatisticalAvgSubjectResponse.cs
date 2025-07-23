namespace NL_THUD.Dtos.Response
{
    public class StatisticalAvgSubjectResponse
    {
        public List<AvgSubjectResponse> AvgSubject_Semester1 { get; set; }
        public List<AvgSubjectResponse> AvgSubject_Semester2 { get; set; }
        public List<AvgSubjectResponse> AvgSubject_Final { get; set; }

    }
}
