using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCompetitorDto
    {
        public int TourCompId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; } // From Tournament
        public string TourPlace { get; set; } // From Tournament
        public DateTime TourDate { get; set; }
        public int CompId { get; set; }
        public int? TourCatKataId { get; set; }
        public string? KataCatName { get; set; }
        public int? TourCatKumiteId { get; set; }
        public string? KumiteCatName { get; set; }
    }
}
