using KarateSystem.Misc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KarateSystem.Models
{
    public class Kata
    {
        public int KataId { get; set; }
        public decimal? KataRate1 { get; set; }
        public decimal? KataRate2 { get; set; }
        public decimal? KataRate3 { get; set; }
        public decimal? KataRate4 { get; set; }
        public decimal? KataRate5 { get; set; }
        public decimal? KataScore { get; set; }
        public int? Overtime { get; set; } // dogrywka
        public int TourCompId { get; set; }
        public TourCompetitor TourCompetitor { get; set; }
    }
}
