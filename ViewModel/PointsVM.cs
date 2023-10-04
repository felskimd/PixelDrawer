using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace PixelDrawer.ViewModel
{
    class PointsVM: INotifyPropertyChanged
    {
        private Point? point3;
        public Point? Point3
        {
            get { return point3; }
            set
            {
                point3 = value;
                OnPropertyChanged("Point3");
            }
        }

        private Point? point2;
        public Point? Point2
        {
            get { return point2; }
            set
            {
                point2 = value;
                OnPropertyChanged("Point2");
            }
        }

        private Point? point1;
        public Point? Point1
        {
            get { return point1; }
            set
            {
                point1 = value;
                OnPropertyChanged("Point1");
            }
        }

        private Point point1TabControl;
        public Point Point1TabControl
        {
            get { return point1TabControl; }
            set
            {
                point1TabControl = value;
                OnPropertyChanged("Point1TabControl");
            }
        }

        private Point currentPoint;
        public Point CurrentPoint
        {
            get { return currentPoint; }
            set
            {
                currentPoint = value;
                OnPropertyChanged("CurrentPoint");
            }
        }

        private Point currentPointTabControl;
        public Point CurrentPointTabControl
        {
            get { return currentPointTabControl; }
            set
            {
                currentPointTabControl = value;
                OnPropertyChanged("CurrentPointTabControl");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
