namespace NL_THUD.Dtos.Request
{
    public class UpdateScoreOfSubjectRequest
    {
        public double? OralScore { get; set; }  // Điểm miệng
        public double? QuizScore { get; set; }  // Điểm 15 phút
        public double? TestScore { get; set; }  // Điểm 1 tiết
        public double? FinalExamScore { get; set; }  // Điểm thi cuối kỳ
    }
}
