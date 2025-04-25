namespace NL_THUD.Dtos.Response
{
    public class EvaluationResponse
    {
        public string year { get; set; }
        public int semester { get; set; }
        public string Evaluation_Content { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? UpdateTime_Expire { get; set; }
    }
}
