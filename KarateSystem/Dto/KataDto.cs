using KarateSystem.Misc;
using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class KataDto
    {
        public int KataId { get; set; }
        public int? Numeration { get; set; }
        public decimal? KataRate1 { get; set; }
        public decimal? KataRate2 { get; set; }
        public decimal? KataRate3 { get; set; }
        public decimal? KataRate4 { get; set; }
        public decimal? KataRate5 { get; set; }
        public decimal? KataScore { get; set; }
        public int? Overtime { get; set; } // dogrywka
        public string OvertimeDisplay => Helper.OvertimePlaceList
            .FirstOrDefault(opt => opt.Value == Overtime)?.DisplayName ?? "Brak";
        public int TourCompId { get; set; }
        public int CompId { get; set; }
        public string CompFirstName { get; set; }
        public string CompLastName { get; set; }
    }
}
