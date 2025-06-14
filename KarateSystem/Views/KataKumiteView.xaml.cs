﻿using KarateSystem.Misc;
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
    /// Interaction logic for KataKumiteView.xaml
    /// </summary>
    public partial class KataKumiteView : UserControl
    {
        public KataKumiteView()
        {
            InitializeComponent();
        }

        private void tbRate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;

            string newText = currentText.Insert(textBox.SelectionStart, e.Text);

            e.Handled = !Helper.IsRateValidDecimal(newText);
        }
    }
}
