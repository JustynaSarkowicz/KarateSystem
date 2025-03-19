using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
    }
}
