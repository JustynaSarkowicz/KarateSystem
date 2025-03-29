using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Fight
    {
        // pomijamy
        public int FightId { get; set; }
        public decimal? FightScoreA { get; set; }
        public decimal? FightScoreB { get; set; }
        public int? FightWinner { get; set; }
        public int? FightTime { get; set; }
        public int? FightNumOverTime { get; set; }
    }
}
