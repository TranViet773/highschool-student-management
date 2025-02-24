using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Districts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Districts_Id { get; set; }
        public string Districts_Name { get; set; }
        public List<Wards> Wards { get; set; }

        public Provinces Provinces { get; set; }
        [ForeignKey("Province_Id")]
        public int Province_Id {  get; set; }

    }
}
