using KarateSystem.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for TournamentView.xaml
    /// </summary>
    public partial class TournamentView : UserControl
    {
        public TournamentView()
        {
            InitializeComponent();
        }

        private void btnEditTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchTour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbSearchTour_LostFocus(object sender, RoutedEventArgs e) => Helper.SetPlaceholderIfEmpty(tbSearchTour);

        private void tbSearchTour_GotFocus(object sender, RoutedEventArgs e) => Helper.ClearPlaceholderOnFocus(tbSearchTour);

        private void btnSearchTourComp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbSearchTourComp_LostFocus(object sender, RoutedEventArgs e) => Helper.SetPlaceholderIfEmpty(tbSearchTourComp);

        private void tbSearchTourComp_GotFocus(object sender, RoutedEventArgs e) => Helper.ClearPlaceholderOnFocus(tbSearchTourComp);

        private void btnFilterTourCompetitor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTourAddComp_Click(object sender, RoutedEventArgs e)
        {
            var AddCompWindow = new AddCompetitorsView();
            AddCompWindow.ShowDialog();
        }

        private void btnTourDeleteComp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTourAddCatKata_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTourAddCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTourDeleteCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
