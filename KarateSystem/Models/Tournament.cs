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
        [Column(TypeName = "Date")]
        public DateTime TourDate{  get; set; }
        public ICollection<KataCategory> TourKataCategories { get; set; } = new List<KataCategory>();
        public ICollection<KumiteCategory> TourKumiteCategories { get; set; } = new List<KumiteCategory>();
        public ICollection<Kata> TourKatas { get; set; } = new List<Kata>();
        public ICollection<Fight> TourFights { get; set; } = new List<Fight>();
    }
}
