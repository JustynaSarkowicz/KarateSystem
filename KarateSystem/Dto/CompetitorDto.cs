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
                var today = DateTime.Today;
                var age = today.Year - CompDateOfBirth.Year;
                if (CompDateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        public bool CompGender { get; set; }
        public decimal CompWeight { get; set; }
        public int CompDegreeId { get; set; }
        public string DegreeName { get; set; }
        public int CompClubId { get; set; }
        public string ClubName { get; set; }
        public ICollection<TourCompetitorDto> TourCompetitors { get; set; } = new List<TourCompetitorDto>();

    }
}
