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
    /// Interaction logic for ClubsDegreesMatsView.xaml
    /// </summary>
    public partial class ClubsDegreesMatsView : UserControl
    {
        public ClubsDegreesMatsView()
        {
            InitializeComponent();
        }

        private void btnSaveClub_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditClub_Click(object sender, RoutedEventArgs e)
        {
            tbClubName.IsReadOnly = false;
            tbClubPlace.IsReadOnly = false;
        }

        private void btnAddNewClub_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveDegree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditDegree_Click(object sender, RoutedEventArgs e)
        {
            tbDegreeName.IsReadOnly = false;
        }

        private void btnAddNewDegree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveMat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditMat_Click(object sender, RoutedEventArgs e)
        {
            tbMatName.IsReadOnly = false;
        }

        private void btnAddNewMat_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
