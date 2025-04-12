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
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
        public ICollection<TourCatKata> TourCatKatas { get; set; } = new List<TourCatKata>();
        public ICollection<TourCatKumite> TourCatKumites { get; set; } = new List<TourCatKumite>();
    }
}
