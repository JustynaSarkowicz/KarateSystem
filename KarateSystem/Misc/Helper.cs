using System.Windows.Controls;

namespace KarateSystem.Misc
{
    public static class Helper
    {
        public static void SetPlaceholderIfEmpty(TextBox textBox, string placeholder)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) textBox.Text = placeholder;
            
        }
        public static void ClearPlaceholderOnFocus(TextBox textBox, string placeholder)
        {
            if (textBox != null && textBox.Text == placeholder) textBox.Text = "";
        }
    }
}
