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
            RejestracjaOtwarta,
            RejestracjaZamknięta,
            Otwarty,
            Zakończony
        }
        public enum Role
        {
            Admin,
            Obsługa
        }
    }
}
