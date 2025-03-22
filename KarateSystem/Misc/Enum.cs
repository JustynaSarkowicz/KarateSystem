using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Misc
{
    public static class Enum
    {
        public enum Gender
        {
            Kobieta,
            Mężczyzna
        }
        public enum Status
        {
            Otwarty,
            RejestracjaOtwarta,
            RejestracjaZamknięta,
            Zakończony
        }
        public enum Role
        {
            Admin,
            Obsługa
        }
    }
}
