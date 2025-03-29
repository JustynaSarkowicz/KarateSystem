using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Mat
    {
        public int MatId { get; set; }
        public string MatName { get; set; }
        public ICollection<TourCatKata> TourCatKatas { get; set; } = new List<TourCatKata>();
        public ICollection<TourCatKumite> TourCatKumites { get; set; } = new List<TourCatKumite>();
    }
}
