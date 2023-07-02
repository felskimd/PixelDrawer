using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;

namespace PixelDrawer.Model
{
    public class Layer : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public WriteableBitmap Bitmap { get; }
        public int Width { get; }
        public int Height { get; }

        private bool isVisible = true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
                OnPropertyChanged("Visibility");
            }
        }

        public Visibility Visibility
        {
            get { return isVisible ? Visibility.Visible : Visibility.Collapsed; }
            set
            {
                if (value == Visibility.Visible)
                    IsVisible = true;
                else
                    IsVisible = false;
                OnPropertyChanged("Visibility");
                OnPropertyChanged("IsVisible");
            }
        }

        public Layer(string name, int width, int height)
        {
            Name = name;
            Bitmap = BitmapFactory.New(width, height);
        }

        public Layer(string name, int width, int height, Color color)
        {
            Name = name;
            Bitmap = BitmapFactory.New(width, height);
            Bitmap.Clear(color);
        }

        public Layer(string name, WriteableBitmap bmp, bool isVisible = true)
        {
            Name = name;
            Bitmap = bmp;
            this.IsVisible = isVisible;

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
