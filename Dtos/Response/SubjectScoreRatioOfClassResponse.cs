namespace NL_THUD.Dtos.Response
{
    public class SubjectScoreRatioOfClassResponse
    {
        public RatioOfScoreType RatioOfOralScore { get; set; }  
        public RatioOfScoreType RatioOfOralScore1 { get; set; }  
        public RatioOfScoreType RatioOfOralScore2 { get; set; }  
        public RatioOfScoreType RatioOfOralScore3 { get; set; }  
        public RatioOfScoreType RatioOfQuizScore { get; set; }  
        public RatioOfScoreType RatioOfQuizScore1 { get; set; }  
        public RatioOfScoreType RatioOfQuizScore2 { get; set; }  
        public RatioOfScoreType RatioOfQuizScore3 { get; set; }  
        public RatioOfScoreType RatioOfMidTermScore { get; set; }  
        public RatioOfScoreType RatioOfFinalScore { get; set; }  
        public RatioOfScoreType RatioOfAverageScore { get; set; }  
        public RatioOfScoreType RatioOfFinalAverageScore { get; set; }  
    }
    public class RatioOfScoreType
    {
        public RatioScore Ratio80to100 { get; set; }  
        public RatioScore Ratio65to79 { get; set; }  
        public RatioScore Ratio50to64 { get; set; }  
        public RatioScore Ratio35to49 { get; set; }  
        public RatioScore Ratio0to34 { get; set; }  
        public RatioScore RatioMoreThan50 { get; set; }  
    }
    public class RatioScore
    {
        public int? Quantity { get; set; } 
        public double? Ratio { get; set; } // Tỷ lệ phần trăm
    }
}
