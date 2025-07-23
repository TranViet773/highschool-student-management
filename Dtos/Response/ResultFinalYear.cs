using NL_THUD.Models.Enum;

namespace NL_THUD.Dtos.Response
{
    public class ResultFinalYear
    {
        public string Comment_Final { get; set; } // Nhận xét
        public EPerformance EPerformance_Final { get; set; } //Học lực
        public double? AVG_Semester_Score_Final { get; set; } // Điểm TB cuối năm
        public int Grade_Final { get; set; } // Hạng
        public EClassification Classification_Final { get; set; } //Khen thưởng
        public EConduct EConduct_Final { get; set; } //Rèn luyện
        public double? AVG_Semester1_Score { get; set; }
        public double? AVG_Semester2_Score { get; set; }
        public bool? isPassClass { get; set; }
    }
}
