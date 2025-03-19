using System.Windows.Controls;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static void SetPlaceholderIfEmpty(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) textBox.Text = "Szukaj...";
            
        }
        public static void ClearPlaceholderOnFocus(TextBox textBox)
        {
            if (textBox != null && textBox.Text == "Szukaj...") textBox.Text = "";
        }
    }
}
