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

        private void btnSearchCatKata_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbSearchCatKata_LostFocus(object sender, RoutedEventArgs e) => Helper.SetPlaceholderIfEmpty(tbSearchCatKata);

        private void tbSearchCatKata_GotFocus(object sender, RoutedEventArgs e) => Helper.ClearPlaceholderOnFocus(tbSearchCatKata);

        private void btnSaveCatKata_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditCatKata_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewCatKata_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearchCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNewCatKumite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbSearchCatKumite_LostFocus(object sender, RoutedEventArgs e) => Helper.SetPlaceholderIfEmpty(tbSearchCatKumite);

        private void tbSearchCatKumite_GotFocus(object sender, RoutedEventArgs e) => Helper.ClearPlaceholderOnFocus(tbSearchCatKumite);


    }
}
