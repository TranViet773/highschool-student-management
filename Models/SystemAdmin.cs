using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace NL_THUD.Models
{
    public class SystemAdmin : Person
    {
        public List<Teacher> Teachers { get; set;} = new List<Teacher>();
        public List<Students> Students { get; set; } = new List<Students>();
        public List<Parents> Parents { get; set; } = new List<Parents>();
    }
}
