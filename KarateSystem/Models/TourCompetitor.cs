using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models
{
    public class TourCompetitor
    {
        // Tabele pośrednie do dodawania zawodników do turnieju
        // Zawodnik może należeć do wielu turniejów, turniej może mieć wielu zawodników
        public int TourCompId { get; set; }
        public int TourId { get; set; } 
        public int CompId { get; set; } 
        public Tournament Tournament { get; set; }
        public Competitor Competitor { get; set; }
    }
}
