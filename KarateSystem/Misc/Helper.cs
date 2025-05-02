using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static bool IsTextNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
        public static bool IsTextValidDecimal(string text)
        {
            return Regex.IsMatch(text, @"^\d*([.]\d{0,2})?$");
        }
        public static bool IsRateValidDecimal(string text)
        {
            return Regex.IsMatch(text, @"^\d*([.]\d{0,1})?$");
        }
        public static List<GenderOption2> GenderOptions2 { get; } = new()
        {
            new GenderOption2("Kobieta", false),
            new GenderOption2("Mężczyzna", true),
            new GenderOption2("Nie wybrano", null)
        };
        public static List<GenderOption> GenderOptions { get; } = new()
        {
            new GenderOption("Kobieta", false),
            new GenderOption("Mężczyzna", true)
        };

        public static string GetDisplayName(bool? isMale) =>
            isMale switch
            {
                true => "Mężczyzna",
                false => "Kobieta",
                null => "Nie wybrano"
            };
        public record GenderOption2(string DisplayName, bool? Value);
        public record GenderOption(string DisplayName, bool Value);
        public static List<StatusOption> StatusOptionsList { get; } = new()
        {       
           new StatusOption("Rejestracja otwarta", 0),
           new StatusOption("Rejestracja zamknięta", 1),
           new StatusOption("Oczekuje na rozpoczęcie", 2),
           new StatusOption("Zakończony", 3),
           new StatusOption("Rozpoczęto turniej", 4)
        };
        public record StatusOption(string DisplayName, int Value);
        public static List<RoleOption> RoleOptionsList { get; } = new()
        {       
           new RoleOption("Obsługa"),
           new RoleOption("Admin")
        };
        public record RoleOption(string DisplayName);
        public static List<OvertimePlaceOption> OvertimePlaceList { get; } = new()
        {
           new OvertimePlaceOption("Brak", 0),
           new OvertimePlaceOption("Miejsce 1", 1),
           new OvertimePlaceOption("Miejsce 2", 2),
           new OvertimePlaceOption("Miejsce 3", 3),
           new OvertimePlaceOption("Miejsce 4", 4)
        };
        public record OvertimePlaceOption(string DisplayName, int Value);
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }
        public static string Decrypt(this string cipherText)
        {
            try
            {
                string EncryptionKey = "skdb";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch { }
            return cipherText;
        }

        public static string Encrypt(this string clearText)
        {
            try
            {
                string EncryptionKey = "skdb";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch { }
            return clearText;
        }
    }
}
