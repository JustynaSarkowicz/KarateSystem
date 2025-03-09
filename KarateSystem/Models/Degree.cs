using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
        public ICollection<KataCategory> DegreeKataCategories { get; set; } = new List<KataCategory>();
        public ICollection<KumiteCategory> DegreeKumiteCategories { get; set; } = new List<KumiteCategory>();
    }
}
