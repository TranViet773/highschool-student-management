using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Parents : Person
    {
        public SystemAdmin SystemAdmin { get; set; }
        [ForeignKey("SystemAdmin_Id")]
        public string SystemAdmin_Id { get; set; }
        //public Guid StudentId { get; set; }
        public Students Students { get; set; }
    }
}
