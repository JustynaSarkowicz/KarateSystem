using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models
{
    public class TourCatKata
    {
        public int TourCatKataId { get; set; }
        public int TourId { get; set; }
        public Tournament Tour { get; set; }
        public int KataCatId { get; set; }
        public KataCategory KataCategory { get; set; }
        public int MatId { get; set; }
        public Mat Mat { get; set; }
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
    }
}
