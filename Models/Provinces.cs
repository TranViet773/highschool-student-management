using NL_THUD.Dtos.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NL_THUD.Models
{
    public class Provinces
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Province_Id { get; set; }
        public string Province_Name { get; set; }

        public List<Districts> Districts { get; set; }
    }
}
