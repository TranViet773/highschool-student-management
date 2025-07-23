using NL_THUD.Models.Enum;
using NL_THUD.Models;

namespace NL_THUD.Dtos.Request
{
    public class ReevaluationRequest
    {
        //public Guid? Reevaluation_Id { get; set; }
        public string? SubjectsId { get; set; }
        public string? StudentsId { get; set; }
        public string? TeacherId { get; set; }
        public string? Reevaluation_ApporverId { get; set; }
        public string? Reevaluation_ScoreColumn { get; set; }
        public double? Reevaluation_PreScore { get; set; }
        public double? Reevaluation_AfterScore { get; set; }
        public int? Reevaluation_Semester { get; set; }
        public string? Reevaluation_Year { get; set; }
        public string? Reevaluation_Reason { get; set; }
        public EStatusSubmittion? Reevaluation_Status { get; set; }
        public string? ClassId { get; set; }
        public DateOnly? Reevaluation_TimestampApprove { get; set; }
        public DateTime? Reevaluation_TimestampEdit { get; set; }
        public DateTime? Reevaluation_TimestampRequest { get; set; }

        //Định nghĩa các quan hệ
    }
}
