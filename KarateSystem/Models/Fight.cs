using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Fight
    {
        public int FightId { get; set; }
        public int TourCatKumiteId { get; set; }
        public TourCatKumite TourCatKumite { get; set; }
        public int? RedCompetitorId { get; set; }
        public TourCompetitor? RedCompetitor { get; set; }
        public int? BlueCompetitorId { get; set; }
        public TourCompetitor? BlueCompetitor { get; set; }
        public int? WinnerId { get; set; }
        public TourCompetitor? Winner { get; set; }
        public decimal? RedCompetitorScore { get; set; }
        public decimal? BlueCompetitorScore { get; set; }
        public int? FightTime { get; set; }
        public int? FightNumOverTime { get; set; }
        public bool FightWalkover { get; set; }
        public int Round { get; set; }
    }
}
