using KarateSystem.Misc;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TournamentDto
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public string TourPlace { get; set; }
        public DateTime TourDate { get; set; }
        public int Status { get; set; }
        public string StatusDisplay => Helper.StatusOptionsList
            .FirstOrDefault(opt => opt.Value == Status)?.DisplayName ?? "Nieznany";
        public int CompetitorCount { get; set; }
        public int KataCategoryCount { get; set; }
        public int KumiteCategoryCount { get; set; }
        public int KumiteCount { get; set; }
        public int KataCount { get; set; }
        public ICollection<TourCompetitorDto> TourCompetitors { get; set; } = new List<TourCompetitorDto>();
        public ICollection<TourCatKataDto> TourCatKatas { get; set; } = new List<TourCatKataDto>();
        public ICollection<TourCatKumiteDto> TourCatKumites { get; set; } = new List<TourCatKumiteDto>();
    }
}
