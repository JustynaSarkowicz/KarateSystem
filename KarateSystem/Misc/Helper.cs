using System.Globalization;
using System.Windows.Controls;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static bool IsTextNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
        public static bool IsValidDecimalInput(string currentText, string newInput, int caretIndex)
        {
            // Calculate what the new text would be
            string potentialText = currentText.Insert(caretIndex, newInput);

            // Allow empty string (for deletion cases)
            if (string.IsNullOrEmpty(potentialText))
                return true;

            // Special handling for decimal point
            if (newInput == ".")
            {
                // Allow only if:
                // 1. No existing dot in the text
                // 2. Not at the start of the text
                return !currentText.Contains('.') && caretIndex > 0;
            }

            // Try parsing the complete potential text
            return decimal.TryParse(potentialText,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out _);
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
