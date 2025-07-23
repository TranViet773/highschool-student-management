using NL_THUD.Models.Enum;

namespace NL_THUD.Dtos.Response
{
    public class EvaluationResponse
    {
        public string year { get; set; }
        public int semester { get; set; }
        public string Evaluation_Content { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? UpdateTime_Expire { get; set; }
        public bool? isPassClass { get; set; } = false; // true: đỗ, false: rớt, null: chưa đánh giá
        public EClassification? Classification { get; set; } = EClassification.NULL;
        public EConduct? Conduct { get; set; } = EConduct.NULL;
        public EPerformance? Performance { get; set; } = EPerformance.NULL;
        public int? Rank { get; set; }
    }
}
