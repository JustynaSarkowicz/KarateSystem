using System.Windows.Controls;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static bool IsTextNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
        public static bool IsWeightCorrect(string text)
        {
            string pattern = @"^(\d*\.?\d{0,2}|\d+\.?)$";

            return System.Text.RegularExpressions.Regex.IsMatch(text, pattern);
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
    }
}
