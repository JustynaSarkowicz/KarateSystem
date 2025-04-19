using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KarateSystem.Misc;
using KarateSystem.Models;
using KarateSystem.Repository.Interfaces;
using KarateSystem.ViewModel;
using static KarateSystem.Misc.Enum;

namespace KarateSystem.Views
{
    /// <summary>
    /// Interaction logic for CompetitorsView.xaml
    /// </summary>
    public partial class CompetitorsView : UserControl
    {
        public CompetitorsView()
        { 
            InitializeComponent();
        }
        private void TbCompetitorWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;

            string newText = currentText.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !Helper.IsTextValidDecimal(newText);
        }
    }
}
