using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Degree
    {
        [Key]
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
        public ICollection<Competitor> Competitors { get; set; } = new List<Competitor>();
        public ICollection<KataCategory> KataCategories { get; set; } = new List<KataCategory>();
    }
}
