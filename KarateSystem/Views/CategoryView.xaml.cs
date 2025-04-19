using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KarateSystem.Misc;

namespace KarateSystem.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!Helper.IsTextNumeric(e.Text)) e.Handled = true;
        }
        private void tbWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;

            string newText = currentText.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !Helper.IsTextValidDecimal(newText);
        }
    }
}
