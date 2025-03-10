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

        [Column(TypeName = "Date")]
        public DateTime CompetitorDateOfBirth { get; set; }

        [NotMapped]
        public int CompetitorAge
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - CompetitorDateOfBirth.Year;
                if (CompetitorDateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        [Column(TypeName = "nvarchar(10)")]
        public Gender CompetitorGender { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        public decimal CompetitorWeight { get; set; }
        public int CompetitorDegreeId { get; set; }
        public Degree CompetitorDegree { get; set; }
        public int CompetitorClubId { get; set; }
        public Club CompetitorClub { get; set; }
        public int? CompetitorKataCategoryId { get; set; }
        public KataCategory? CompetitorKataCategory { get; set; }
        public int? CompetitorKumiteCategoryId { get; set; }
        public KumiteCategory? CompetitorKumiteCategory { get; set; }
        public ICollection<Tournament> TournamentList { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
