using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class FightDto
    {
        public int FightId { get; set; }
        public int FightNumber { get; set; }
        public int TourCatKumiteId { get; set; }
        public int? RedCompetitorId { get; set; }
        public string? RedCompetitorName { get; set; }
        public int? BlueCompetitorId { get; set; }
        public string? BlueCompetitorName { get; set; }
        public int? WinnerId { get; set; }
        public string? WinnerName { get; set; }
        public decimal? RedCompetitorScore { get; set; }
        public decimal? BlueCompetitorScore { get; set; }
        public int? FightTime { get; set; }
        public int? FightNumOverTime { get; set; }
        public bool FightWalkover { get; set; }
        public string FightWalkoverDisplay => FightWalkover ? "Tak" : "Nie";
        public int Round { get; set; }
    }

}
