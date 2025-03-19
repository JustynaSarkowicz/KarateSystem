using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace KarateSystem.Models
{
    public class Competitor
    {
        public int CompetitorId { get; set; }
        public string CompetitorFirstName { get; set; }
        public string CompetitorLastName { get; set; }
        public DateTime CompetitorDateOfBirth { get; set; }
        public int CompetitorAge
        {
            set
            {
                var today = DateTime.Today;
                var age = today.Year - CompetitorDateOfBirth.Year;
                if (CompetitorDateOfBirth.Date > today.AddYears(-age)) age--;
                ;
            }
        }
        public string CompetitorGender { get; set; }
        public decimal CompetitorWeight { get; set; }
        public int CompetitorDegreeId { get; set; }
        public int CompetitorClubId { get; set; }
    }
}
