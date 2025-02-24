using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Address
    {
        [Key]
        public Guid Address_Id { get; set; } = Guid.NewGuid();
        public string Address_Detail { get; set; }

        public Provinces Provinces { get; set; }
        [ForeignKey("Province_Id")]
        public int Province_Id {  get; set; }

        public Person Person { get; set; }

        [ForeignKey("Person_Id")]
        public Guid Person_Id { get; set; }
    }
}
