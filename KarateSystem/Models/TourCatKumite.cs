using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models
{
    public class TourCatKumite
    {
        public int TourCatKumiteId { get; set; }
        public int TourId { get; set; }
        public Tournament Tour { get; set; }
        public int KumiteCatId { get; set; }
        public KumiteCategory KumiteCategory{ get; set; }
        public int MatId { get; set; }
        public Mat Mat { get; set; }
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
    }
}
