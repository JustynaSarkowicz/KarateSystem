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

        public static string GetDisplayName(bool? isMale) =>
            isMale switch
            {
                true => "Mężczyzna",
                false => "Kobieta",
                null => "Nie wybrano"
            };

        public record GenderOption(string DisplayName, bool? Value);
    }
}
