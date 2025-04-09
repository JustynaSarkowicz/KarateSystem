using System.Windows;
using System.Windows.Controls;
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
        private void WeightValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

                if (e.Text == "." && !textBox.Text.Contains("."))
                {
                    return;
                }

                if (!Helper.IsWeightCorrect(newText))
                {
                    e.Handled = true;
                }
            }
        }
    }
}
