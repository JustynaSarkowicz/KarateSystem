using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class KumiteCategory
    {
        public int KumiteCatId { get; set; }
        public string KumiteCatName{ get; set; }
        public string KumiteCatGender { get; set; }
        public int KumiteCatAgeMin { get; set; }
        public int KumiteCatAgeMax { get; set; }
        public decimal KumiteCatWeightMin { get; set; }
        public decimal KumiteCatWeightMax { get; set; }
        public ICollection<TourCatKumite> TourCatKumites { get; set; } = new List<TourCatKumite>();
    }

}
