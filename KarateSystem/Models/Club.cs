using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Club
    {
        [Key]
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubCity { get; set; }
        public ICollection<Competitor> ClubCompetitors { get; set;} = new List<Competitor>();
    }
}
