using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void btnEditCompetitor_Click(object sender, RoutedEventArgs e)
        {
            tbCompetitorName.IsReadOnly = false;
            tbCompetitorSurname.IsReadOnly = false;
            tbCompetitorDateOfBirth.IsReadOnly = false;
            tbCompetitorDateOfBirth.IsReadOnly = false;
            tbCompetitorAge.IsReadOnly = false;
            tbCompetitorWeight.IsReadOnly = false;
            cbCompetitorGender.IsEnabled = true;
            cbCompetitorDegree.IsEnabled = true;
            cbCompetitorClub.IsEnabled = true;
        }

        private void btnSaveCompetitor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchCompetitor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFilterCompetitor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewCompetitor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbSearchCompetitor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSearchCompetitor.Text))
            {
                tbSearchCompetitor.Text = "Szukaj...";
            }
        }

        private void tbSearchCompetitor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearchCompetitor.Text == "Szukaj...")
            {
                tbSearchCompetitor.Text = "";
            }
        }

        private void tbCompetitorDateOfBirth_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text.Trim();

            text = Regex.Replace(text, @"\D", "");
            if (text.Length >= 3)
            {
                text = text.Insert(2, "/");
                if (text.Length > 5)
                {
                    text = text.Insert(5, "/");
                }
            }

            textBox.Text = text;
            textBox.CaretIndex = text.Length;
        }

        private void tbCompetitorDateOfBirth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string text = textBox.Text + e.Text;

            if (text.Length > 10)
            {
                e.Handled = true;
            }
        }
    }
}
