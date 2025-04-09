using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models
{
    public class CatKataDegree
    {
        public int CatKataDegreeId { get; set; }
        public int KataCatId { get; set; }
        public KataCategory KataCategory { get; set; }
        public int DegreeId { get; set; }
        public Degree Degree { get; set; }
    }
}
