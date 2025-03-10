using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubCity { get; set; }
    }
}
