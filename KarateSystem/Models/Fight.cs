using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Fight
    {
        [Key]
        public int FightId { get; set; }
        public int FightTourId { get; set; }
        public Tournament FightTour { get; set; }
        public int FightKumiteCategoryId { get; set; }
        public KumiteCategory FightKumiteCat { get; set; }
        public int FightMatId { get; set; }
        public Mat FightMat { get; set; }
        public int FightCompetitorAId { get; set; }
        public Competitor FightCompetitorA { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal FightScoreA { get; set; }
        public int FightCompetitorBId { get; set; }
        public Competitor FightCompetitorB { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal FightScoreB { get; set; }
        public int FightWinner { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan FightTime { get; set; }
        public int? FightNumOverTime { get; set; }

    }
}
