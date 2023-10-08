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

namespace PixelDrawer.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для CustomBorder.xaml
    /// </summary>
    public partial class CustomBorder : Border
    {
        private Pen pen = new Pen(Brushes.Black, 0.01);

        public CustomBorder()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var horRodCnt = 100;
            var verRodCnt = 100;
            for (var i = 1; i < horRodCnt; i++)
            {
                var offsetX = i;

                dc.DrawLine(pen, new Point(offsetX, 0), new Point(offsetX, ActualHeight));
            }
            for (var i = 1; i < verRodCnt; i++)
            {
                var offsetY = i;

                dc.DrawLine(pen, new Point(0, offsetY), new Point(ActualWidth, offsetY));
            }
        }
    }
}
