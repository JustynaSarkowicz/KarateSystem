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
    }
}
