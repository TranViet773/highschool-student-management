namespace NL_THUD.Dtos.Response
{
    public class StatisticalResultOfGradeResponse
    {
        public string Year { get; set; }
        public List<ResultLearningResponse> Learnings { get; set; }
        public List<ResultPracticeResponse> Practices { get; set; }
        public List<ResultRewardResponse> Reward { get; set; }
        public List<ResultNotPassResponse> NotPassedClass { get; set; }
    }

    public class ResultLearningResponse
    {
        public string ClassCode { get; set; } // Khi thống kê 1 năm thì dùng
        public double Excelent { get; set; }
        public double Good { get; set; }
        public double Passed { get; set; }
        public double NotPassed { get; set; }
        public string Year { get; set; } // Khi thống kê nhiều năm thì dùng
    }

    public class ResultPracticeResponse
    {
        public string ClassCode { get; set; }
        public double Excelent { get; set; }
        public double Good { get; set; }
        public double Passed { get; set; }
        public double NotPassed { get; set; }
        public string Year { get; set; }
    }   
    public class ResultRewardResponse
    {
        public string ClassCode { get; set; }
        public double Excelent { get; set; }
        public double Good { get; set; }
        public string Year { get; set; }
    }

    public class ResultNotPassResponse
    {
        public string ClassCode { get; set; }
        public int NotPass { get; set; }
        public string Year { get; set; }
    }
}
