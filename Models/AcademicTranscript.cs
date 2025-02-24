using NL_THUD.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class AcademicTranscript
    {
        [Key]
        public Guid AcademicTranscript_Id { get; set; } = Guid.NewGuid();
        public string Comment { get; set; }
        public int Great { get; set; } // Thứ hạng
        public string Note { get; set; }
        public required AcademicTranscript_Performance Performance  { get; set; } // Học Lực
        public AcademicTranscript_Conduct Conduct {  get; set; } // Hạnh kiểm
        public float AVG_1st { get; set; }
        public float AVG_2st { get;set; }
        public float AVG_Final { get; set; }

        public List<Student_Score> StudentScores { get; set; } = new List<Student_Score>();
        public Teacher Teacher { get; set; }
        [ForeignKey("TeacherId")]
        public string TeacherId { get; set; }
    }
}
