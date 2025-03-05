using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Kata
    {
        [Key]
        public int KataId { get; set; }
        public int KataTourId { get; set; }
        public Tournament KataTour { get; set; }
        public int KataCategoryId { get; set; }
        public KataCategory KataCategory { get; set; }
        public int KataMatId { get; set; }
        public Mat KataMat { get; set; }
        public int KataCompetitorId { get; set; }
        public Competitor KataCompetitor { get; set; }
        [Column(TypeName = "decimal(3, 1)")] 
        public decimal KataRate1 { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal KataRate2 { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal KataRate3 { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal KataRate4 { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal KataRate5 { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal KataScore { get; set; }
    }
}
