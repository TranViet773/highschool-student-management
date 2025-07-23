using NL_THUD.Models;
using NL_THUD.Models.Enum;

namespace NL_THUD.Dtos.Response
{
    public class ScoreBoardResponse
    {
        public string year { get; set; }
        public int semester { get; set; }
        public string Comment { get; set; } // Đánh giá
        public EPerformance EPerformance { get; set; } //Học lực    
        public double? AVG_Semester_Score { get; set;}
        public int Grade { get; set; } // Hạng
        public EClassification Classification { get; set; } // Xep loai
        public EConduct EConduct { get; set; } //Hạnh Kiểm
        public List<ScoreOfSubjectResponse> scoreOfSubjectResponses { get; set; } // Danh sách các điểm các môn
        public List<FinalSubjectAverageResponse> finalSubjectAverageScore { get; set; }
        public string Comment_Final { get; set; } // Đánh giá
        public EPerformance EPerformance_Final { get; set; } //Học lực
        public double AVG_Semester_Score_Final { get; set; }
        public int Grade_Final { get; set; } // Hạng
        public EClassification Classification_Final { get; set; } // Xep loai
        public EConduct EConduct_Final { get; set; } //Hạnh Kiểm

        public Students? Student { get; set; }
    }
}
