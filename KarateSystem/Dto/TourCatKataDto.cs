using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCatKataDto
    {
        public int TourCatKataId { get; set; }
        public int TourId { get; set; }
        public int KataCatId { get; set; }
        public string KataCatName { get; set; }
        public bool? KataCatGender { get; set; } 
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
        public int MatId { get; set; }
        public string MatName { get; set; }
    }
}
