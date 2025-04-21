using KarateSystem.Misc;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class CompetitorDto
    {
        public int CompId { get; set; }
        public string CompFirstName { get; set; }
        public string CompLastName { get; set; }
        public DateTime CompDateOfBirth { get; set; }
        public int CompAge
        {
            get
            {
                return Helper.CalculateAge(CompDateOfBirth);
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
        public ICollection<TourCompetitorDto> TourCompetitors { get; set; } = new List<TourCompetitorDto>();

    }
}
