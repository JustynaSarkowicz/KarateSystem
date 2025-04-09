using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using static KarateSystem.Misc.Enum;

namespace KarateSystem.Models
{
    public class Competitor
    {
        public int CompId { get; set; }
        public string CompFirstName { get; set; }
        public string CompLastName { get; set; }
        public DateTime CompDateOfBirth { get; set; }
        public int CompAge
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - CompDateOfBirth.Year;
                if (CompDateOfBirth.Date > today.AddYears(-age)) age--; 
                return age;
            }
        }
        public bool CompGender { get; set; }
        public decimal CompWeight { get; set; }
        public int CompDegreeId { get; set; }
        public Degree Degree { get; set; }
        public int CompClubId { get; set; }
        public Club Club { get; set; }
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
    }
}
