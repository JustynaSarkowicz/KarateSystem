using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static bool IsTextNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
        public static List<GenderOption> GenderOptions { get; } = new()
        {
            new GenderOption("Kobieta", false),
            new GenderOption("Mężczyzna", true),
            new GenderOption("Nie wybrano", null)
        };
        public static List<GenderOption2> GenderOptions2 { get; } = new()
        {
            new GenderOption2("Kobieta", false),
            new GenderOption2("Mężczyzna", true)
        };

        public static string GetDisplayName(bool? isMale) =>
            isMale switch
            {
                true => "Mężczyzna",
                false => "Kobieta",
                null => "Nie wybrano"
            };
        public record GenderOption(string DisplayName, bool? Value);
        public record GenderOption2(string DisplayName, bool Value);
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
