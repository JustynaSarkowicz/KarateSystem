using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class KumiteCategoryDto
    {
        public int KumiteCatId { get; set; }
        public string KumiteCatName { get; set; }
        public bool KumiteCatGender { get; set; }
        public int KumiteCatAgeMin { get; set; }
        public int KumiteCatAgeMax { get; set; }
        public decimal KumiteCatWeightMin { get; set; }
        public decimal KumiteCatWeightMax { get; set; }
        public string GenderDisplay => KumiteCatGender switch
        {
            true => "Mężczyzna",
            false => "Kobieta",
        };
    }
}
