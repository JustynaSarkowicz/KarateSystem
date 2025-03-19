using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Tournament
    {
        [Key]
        public int TourId { get; set; }
        public string TourName{ get; set; }
        public string TourPlace { get; set; }
        public DateTime TourDate{  get; set; }
        public string Status{  get; set; }
    }
}
