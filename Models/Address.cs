using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Address
    {
        [Key]
        public Guid Address_Id { get; set; } = Guid.NewGuid();
        public string Address_Detail { get; set; }

        [ForeignKey("Wards")]
        public int Ward_Id { get; set; }
        public Wards Wards { get; set; }

        [ForeignKey("Person")]
        public string Person_Id { get; set; }
        public Person Person { get; set; }
    }
}
