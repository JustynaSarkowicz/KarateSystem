using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserLogin { get; set; }
        public string UserPass { get; set; }
        public string UserRole { get; set; }
        public string DisplayName => $"{UserFirstName} {UserLastName}";
    }
}
