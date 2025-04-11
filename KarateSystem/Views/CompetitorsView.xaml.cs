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
        //private void tbCompetitorDateOfBirth_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    string text = textBox.Text.Trim();

        //    text = Regex.Replace(text, @"\D", "");
        //    if (text.Length >= 3)
        //    {
        //        text = text.Insert(2, "/");
        //        if (text.Length > 5)
        //        {
        //            text = text.Insert(5, "/");
        //        }
        //    }

        //    textBox.Text = text;
        //    textBox.CaretIndex = text.Length;
        //}

        //private void tbCompetitorDateOfBirth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    TextBox textBox = (TextBox)sender;
        //    string text = textBox.Text + e.Text;

        //    if (text.Length > 10)
        //    {
        //        e.Handled = true;
        //    }
        //}
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
