using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCatKumiteDto
    {
        public int TourCatKumiteId { get; set; }
        public int TourId { get; set; }
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
        public int MatId { get; set; }
        public string MatName { get; set; }
        public ICollection<Fight> Fights { get; set; } = new List<Fight>();
    }
}
