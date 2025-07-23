using CloudinaryDotNet.Actions;
using NL_THUD.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace NL_THUD.Models
{
    public class Reevaluation
    {
        [Key]
        public Guid Reevaluation_Id { get; private set; } = Guid.NewGuid();
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
        public DateTime? Reevaluation_TimestampExpiry { get; set; } // thời hạn chỉnh điểm

        //Định nghĩa các quan hệ
        public string StudentsId { get; set; }
        public Students Students { get; set; }
        public string? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }  // <== Điều hướng đến Teacher
        public Guid SubjectsId { get; set; }
        public Subjects Subjects { get; set; }
        public string ClassId { get; set; }
    }
}
