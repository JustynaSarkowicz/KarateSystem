using KarateSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class TourCatKataDto
    {
        public int TourCatKataId { get; set; }
        public int TourId { get; set; }
        public int KataCatId { get; set; }
        public string KataCatName { get; set; }
        public int MatId { get; set; }
    }
}
