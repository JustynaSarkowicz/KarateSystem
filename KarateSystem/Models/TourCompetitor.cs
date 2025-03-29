using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models
{
    public class TourCompetitor
    {
        // Tabele pośrednie do dodawania zawodników do turnieju i kategorii dla nich
        public int TourCompId { get; set; }
        public int TourId { get; set; }
        public Tournament Tournament { get; set; }
        public int CompId { get; set; } 
        public Competitor Competitor { get; set; }
        public int TourCatKataId { get; set; }
        public TourCatKata TourCatKata { get; set; }
        public int TourCatKumiteId { get; set; }
        public TourCatKumite TourCatKumite { get; set; }
        public Kata? Kata { get; set; }
    }
}
