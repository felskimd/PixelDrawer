using PixelDrawer.Model;
using PixelDrawer.ViewModel;
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

namespace PixelDrawer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void Grid_MouseMove(object sender, MouseEventArgs e)
        //{
        //    var mainWindowVM = this.DataContext as MainWindowVM;
        //    if (mainWindowVM.Tools.SelectedTool is TestPencilTool)
        //    {
        //        var position = e.GetPosition(sender as Grid);
        //        mainWindowVM.drawingBrush.Margin = new Thickness(
        //            position.X - mainWindowVM.drawingBrush.Width/2, position.Y - mainWindowVM.drawingBrush.Width / 2, 0, 0);
        //    }
        //}

        //private void Grid_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    var mainWindowVM = this.DataContext as MainWindowVM;
        //    var grid = sender as Grid;
        //    grid.Children.Remove(mainWindowVM.drawingBrush);
        //}

        //private void Grid_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    var mainWindowVM = this.DataContext as MainWindowVM;
        //    //var drawingBrush = mainWindowVM.drawingBrush;
        //    mainWindowVM.drawingBrush = new Ellipse();
        //    mainWindowVM.drawingBrush.HorizontalAlignment = HorizontalAlignment.Left;
        //    mainWindowVM.drawingBrush.VerticalAlignment = VerticalAlignment.Top;
        //    mainWindowVM.drawingBrush.Stroke = Brushes.Black;
        //    mainWindowVM.drawingBrush.Width = 5;
        //    mainWindowVM.drawingBrush.Height = 5;
        //    mainWindowVM.drawingBrush.StrokeThickness = 1;
        //    var grid = sender as Grid;
        //    grid.Children.Add(mainWindowVM.drawingBrush);
        //}
    }
}
