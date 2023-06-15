using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace PixelDrawer.ViewModel
{
    class ColorsVM: INotifyPropertyChanged
    {
        private Color selectedColor;
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                RecentColors.Add(new ColorItem(selectedColor, ""));
                OnPropertyChanged("SelectedColor");
            }
        }

        public ObservableCollection<ColorItem> RecentColors { get; private set; }

        public ColorsVM()
        {
            RecentColors = new ObservableCollection<ColorItem>();
            selectedColor = Colors.White;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
