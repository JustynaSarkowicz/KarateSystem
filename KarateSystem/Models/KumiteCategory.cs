using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class KumiteCategory
    {
        [Key]
        public int KumiteCategoryId { get; set; }
        public int KumiteCategoryTourId { get; set; }
        public Tournament KumiteCategoryTour { get; set; }
        public string KumiteCategoryName{ get; set; }
        public string KumiteCategoryGender { get; set; }
        public int KumiteCategoryAgeMin { get; set; }
        public int KumiteCategoryAgeMax { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal KumiteCategoryWeightMin { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal KumiteCategoryWeightMax { get; set; }
        public ICollection<Degree> KumiteCategoryDegrees { get; set; }
        public ICollection<Competitor> KumiteCategoryCompetitors { get; set; } = new List<Competitor>();

    }

}
