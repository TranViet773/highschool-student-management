using NL_THUD.Models.Enum;
using NL_THUD.Models;

namespace NL_THUD.Dtos.Response
{
    public class ReevaluationResponse
    {
        public Guid Reevaluation_Id { get; private set; }
        public string SubjectsId { get; set; }
        public string StudentsId { get; set; }
        public string? TeacherId { get; set; }
        public string? Reevaluation_ApporverId { get; set; }
        public string? Reevaluation_ScoreColumn { get; set; }
        public double Reevaluation_PreScore { get; set; }
        public double? Reevaluation_AfterScore { get; set; }
        public int Reevaluation_Semester { get; set; }
        public string Reevaluation_Year { get; set; }
        public string Reevaluation_Reason { get; set; }
        public EStatusSubmittion Reevaluation_Status { get; set; }
        public DateTime? Reevaluation_TimestampApprove { get; set; }
        public DateTime? Reevaluation_TimestampEdit { get; set; }
        public DateTime? Reevaluation_TimestampRequest { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
        public string? StudentEmail { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherEmail { get; set; }
        public string? TeacherCode { get; set; }
        public string? ClassId { get; set; }
    }
}
