using NL_THUD.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Evaluation
    {
        [Key]
        public Guid Evaluation_Id { get; set; } = Guid.NewGuid();

        public string year { get; set; }
        public int semester {  get; set; }
        public string Evaluation_Content { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? UpdateTime_Expire { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Students Student { get; set; }
    }
}
