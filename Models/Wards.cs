using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Wards
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Ward_Id { get; set; }
        public string Ward_Name { get; set; }

        public List<Address> Addresses = new List<Address>();

        //Relationship
        [ForeignKey("Districts_Id")]
        public int Districts_Id { get; set; }
        public Districts Districts { get; set; }
    }
}
