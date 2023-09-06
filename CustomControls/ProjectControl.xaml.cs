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
    /// Логика взаимодействия для ProjectControl.xaml
    /// </summary>
    public partial class ProjectControl : UserControl
    {
        protected bool isDragging;  
        private Point clickPosition;
        
        public ProjectControl()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            this.MouseMove += new MouseEventHandler(Control_MouseMove);
            this.MouseWheel += new MouseWheelEventHandler(Control_MouseWheel);
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            var draggableControl = sender as UserControl;
            clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(this.Parent as UIElement);

                var transform = draggableControl.RenderTransform as TranslateTransform;
                if (transform == null)
                {
                    transform = new TranslateTransform();
                    draggableControl.RenderTransform = transform;
                }

                transform.X = currentPosition.X - clickPosition.X;
                transform.Y = currentPosition.Y - clickPosition.Y;
            }
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scalingControl = sender as UserControl;
            var transform = scalingControl.RenderTransform as ScaleTransform;
            if (transform == null)
            {
                transform = new ScaleTransform();
                scalingControl.RenderTransform = transform;
            }
            var zoomCenter = e.GetPosition(this);
            transform.CenterX = zoomCenter.X;
            transform.CenterY = zoomCenter.Y;
            transform.ScaleX += e.Delta;
            transform.ScaleY += e.Delta;
        }

        public bool IsDragging() => this.isDragging;
    }
}
