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
using System.Windows.Shapes;
using PixelDrawer.ViewModel;

namespace PixelDrawer.View
{
    /// <summary>
    /// Логика взаимодействия для CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
        }
        //Implement in vm
        private void Numbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int value;
            if (!Int32.TryParse(e.Text, out value))
            {
                e.Handled = true;
            }
        }

        private void Numbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void Numbers_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBoxSender = (TextBox)sender;
            int value;
            if (int.TryParse(textBoxSender.Text, out value))
                if (value > 2048) textBoxSender.Text = "2048";
        }
    }
}
