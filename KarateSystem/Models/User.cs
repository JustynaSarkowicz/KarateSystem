using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName{ get; set; }
        public string UserLogin{ get; set; }
        public string UserPass{ get; set; }
        public string UserRole{ get; set; }
    }
}
