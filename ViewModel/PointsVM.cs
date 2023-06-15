using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PixelDrawer.ViewModel
{
    class PointsVM: INotifyPropertyChanged
    {
        private Point oldPoint;
        public Point OldPoint
        {
            get { return oldPoint; }
            set
            {
                oldPoint = value;
                OnPropertyChanged("OldPoint");
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

        private Point zoomCenterPoint;
        public Point ZoomCenterPoint
        {
            get { return zoomCenterPoint; }
            set
            {
                zoomCenterPoint = value;
                OnPropertyChanged("ZoomCenterPoint");
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
