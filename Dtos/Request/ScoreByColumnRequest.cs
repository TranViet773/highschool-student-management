namespace NL_THUD.Dtos.Request
{
    public class ScoreByColumnRequest
    {
        public string studentId { get; set; }
        public double score { get; set; }
        public string column { get; set; }
        public string comment { get; set; }
    }
}
