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

        private void tbSearchCompetitor_LostFocus(object sender, RoutedEventArgs e) => Helper.SetPlaceholderIfEmpty(tbSearchCompetitor);

        private void tbSearchCompetitor_GotFocus(object sender, RoutedEventArgs e) => Helper.ClearPlaceholderOnFocus(tbSearchCompetitor);

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

        private void dgvCompetitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
