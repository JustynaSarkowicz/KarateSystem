using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace KarateSystem.Models
{
    public class Competitor
    {
        public int CompId { get; set; }
        public string CompFirstName { get; set; }
        public string CompLastName { get; set; }
        public DateTime CompDateOfBirth { get; set; }
        public bool CompGender { get; set; }
        public decimal CompWeight { get; set; }
        public int CompDegreeId { get; set; }
        public Degree Degree { get; set; }
        public int CompClubId { get; set; }
        public Club Club { get; set; }
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
    }
}
