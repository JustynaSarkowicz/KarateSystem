using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCompetitorDto
    {
        public int TourCompId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; } // From Tournament
        public string TourPlace { get; set; } // From Tournament
        public DateTime TourDate { get; set; }
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
        public string GenderDisplay => CompGender switch
        {
            true => "Mężczyzna",
            false => "Kobieta",
        };
        public decimal CompWeight { get; set; }
        public int CompDegreeId { get; set; }
        public string DegreeName { get; set; }
        public int CompClubId { get; set; }
        public string ClubName { get; set; }
        public int? TourCatKataId { get; set; }
        public string? KataCatName { get; set; }
        public int? TourCatKumiteId { get; set; }
        public string? KumiteCatName { get; set; }
        public int? KataId { get; set; }
    }
}
