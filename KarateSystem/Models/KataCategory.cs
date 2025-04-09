using KarateSystem.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class KataCategory
    {
        public int KataCatId { get; set; }
        public string KataCatName { get; set; }
        public bool? KataCatGender { get; set; } // true - male, false - female
        public int KataCatAgeMin { get; set; }
        public int KataCatAgeMax { get; set; }
        public ICollection<CatKataDegree> CatKataDegrees { get; set; } = new ObservableCollection<CatKataDegree>();
        public ICollection<TourCatKata> TourCatKatas { get; set; } = new List<TourCatKata>();
    }
}
