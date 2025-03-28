using NL_THUD.Models.Enum;

namespace NL_THUD.Dtos.Response
{
    public class ScoreBoardResponse
    {
        public string Comment { get; set; } // Đánh giá
        public EPerformance EPerformance { get; set; } //Học lực
        public double AVG_Semester_Score { get; set;}
        public int Grade { get; set; } // Hạng
        public EClassification Classification { get; set; } // Xep loai
        public EConduct EConduct { get; set; } //Hạnh Kiểm
        public List<ScoreOfSubjectResponse> scoreOfSubjectResponses { get; set; } // Danh sách các điểm các môn
    }
}
