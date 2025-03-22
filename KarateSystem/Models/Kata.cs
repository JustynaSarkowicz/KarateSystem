using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Kata
    {
        public int KataId { get; set; }
        public int KataMatId { get; set; }
        public Mat Mat { get; set; }
        public decimal? KataRate1 { get; set; }
        public decimal? KataRate2 { get; set; }
        public decimal? KataRate3 { get; set; }
        public decimal? KataRate4 { get; set; }
        public decimal? KataRate5 { get; set; }
        public decimal? KataScore { get; set; }
    }
}
