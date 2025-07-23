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
        public int semester {  get; set; } // cả năm: 0
        public string Evaluation_Content { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? isPassClass { get; set; } = false; // true: đỗ, false: rớt, null: chưa đánh giá
        public EClassification? Classification { get; set; } = EClassification.NULL;
        public EConduct? Conduct { get; set; } = EConduct.NULL;
        public EPerformance? Performance { get; set; } = EPerformance.NULL;
        public int? Rank { get; set; }
        //public DateTime? UpdateTime_Expire { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        public Students Student { get; set; }
    }
}
