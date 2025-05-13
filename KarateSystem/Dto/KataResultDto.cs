using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class KataResultDto
    {
        public int Place { get; set; }
        public string FullName { get; set; }
        public string ClubName { get; set; }
        public string CategoryName { get; set; }
        public decimal? Score { get; set; }
        public int? Overtime { get; set; }
    }
}
