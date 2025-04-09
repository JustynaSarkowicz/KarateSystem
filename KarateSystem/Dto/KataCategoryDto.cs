using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class KataCategoryDto
    {
        public int KataCatId { get; set; }
        public string KataCatName { get; set; }
        public bool? KataCatGender { get; set; } // true - Mężczyzna, false - Kobieta
        public int KataCatAgeMin { get; set; }
        public int KataCatAgeMax { get; set; }
        public ICollection<CatKataDegreeDto> CatKataDegrees { get; set; } = new List<CatKataDegreeDto>();

        public string DegreesDisplay => string.Join(", ",
            CatKataDegrees?.Select(x => x.DegreeName) ?? Enumerable.Empty<string>());

        public string GenderDisplay => KataCatGender switch
        {
            true => "Mężczyzna",
            false => "Kobieta",
            null => "Nie wybrano"
        };
    }

}
