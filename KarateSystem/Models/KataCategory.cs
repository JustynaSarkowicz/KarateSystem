using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class KataCategory
    {
        public int KataCatId { get; set; }
        public string KataCatName { get; set; } 
        public string KataCatGender { get; set; }
        public int KataCatAgeMin { get; set; }
        public int KataCatAgeMax { get; set; }
        public int KataCatDegreeId { get; set; }
        public Degree Degree { get; set; }
        public ICollection<TourCatKata> TourCatKatas { get; set; } = new List<TourCatKata>();

    }
}
