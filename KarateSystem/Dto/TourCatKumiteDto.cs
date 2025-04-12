using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCatKumiteDto
    {
        public int TourCatKumiteId { get; set; }
        public int TourId { get; set; }
        public int KumiteCatId { get; set; }
        public string KumiteCatName { get; set; }
        public int MatId { get; set; }
    }
}
