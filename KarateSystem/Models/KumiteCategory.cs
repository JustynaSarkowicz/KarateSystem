using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class KumiteCategory
    {
        [Key]
        public int KumiteCategoryId { get; set; }
        public string KumiteCategoryName{ get; set; }
        public string KumiteCategoryGender { get; set; }
        public int KumiteCategoryAgeMin { get; set; }
        public int KumiteCategoryAgeMax { get; set; }
        public decimal KumiteCategoryWeightMin { get; set; }
        public decimal KumiteCategoryWeightMax { get; set; }
    }

}
