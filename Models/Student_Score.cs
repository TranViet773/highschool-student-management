using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Student_Score
    {
        public Students Students { get; set; }
        [ForeignKey("StudentId")]
        public string StudentId { get; set; }   

        public Score Score { get; set; }
        [ForeignKey("ScoreId")]
        public Guid ScoreId { get; set; }

        public Subjects Subjects { get; set; }
        [ForeignKey("SubjectId")]
        public Guid SubjectId { get; set; }

        public AcademicTranscript AcademicTranscript { get; set; }
        [ForeignKey("ATId")]
        public Guid ATId { get; set; }

        public string Year { get; set; }
        public int Semester {  get; set; }  
    }
}
