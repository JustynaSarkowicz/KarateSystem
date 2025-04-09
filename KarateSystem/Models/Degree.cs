using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Degree
    {
        public int DegreeId { get; set; }
        public string DegreeName { get; set; }
        public ICollection<Competitor> Competitors { get; set; } = new List<Competitor>();
        public ICollection<CatKataDegree> CatKataDegrees { get; set; } = new List<CatKataDegree>();
    }
}
