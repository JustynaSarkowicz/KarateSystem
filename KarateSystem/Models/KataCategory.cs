using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class KataCategory
    {
        [Key]
        public int KataCategoryId { get; set; }
        public string KataCategoryName { get; set; } 
        public string? KataCategoryGender { get; set; }
        public int KataCategoryAgeMin { get; set; }
        public int KataCategoryAgeMax { get; set; }
        public ICollection<Degree> KataCategoryDegrees { get; set; } = new List<Degree>();
        public ICollection<Competitor> KataCategoryCompetitors { get; set; } = new List<Competitor>();
    }
}
