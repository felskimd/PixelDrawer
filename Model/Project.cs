using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PixelDrawer.Model
{
    public class Project : INotifyPropertyChanged
    {
        public string Name { get; }
        public ObservableCollection<Layer> Layers { get; private set; }

        private int newLayersCount = 1;
        private int NewLayersCount
        {
            get
            {
                newLayersCount++;
                return newLayersCount;
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private Layer selectedLayer;
        public Layer SelectedLayer
        {
            get { return selectedLayer; }
            set
            {
                selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        private double scale = 1.0;
        public double Scale
        {
            get => scale;
            set
            {
                scale = value;
                OnPropertyChanged("Scale");
            }
        }

        public Project(string name, Color backgroundColor, int width, int height)
        {
            Name = name;
            Layers = new ObservableCollection<Layer>
            {
                new Layer("Background", width, height, backgroundColor)
            };
            Width = width;
            Height = height;
            SelectedLayer = Layers[0];
        }

        public Project(string name, WriteableBitmap bmp)
        {
            Name = name;
            Layers = new ObservableCollection<Layer>
            {
                new Layer("Background", bmp)
            };
            Width = (int)bmp.Width;
            Height = (int)bmp.Height;
            SelectedLayer = Layers[0];
        }

        public Project(string name, ObservableCollection<Layer> layers, int newLayersCount, int width, int height)
        {
            Name = name;
            Layers = layers;
            this.newLayersCount = newLayersCount;
            Width = width;
            Height = height;
            SelectedLayer = selectedLayer;
        }

        public Layer AddLayer()
        {
            Layers.Add(new Layer(NewLayersCount.ToString(), Width, Height));
            SelectedLayer = Layers.Last();
            return SelectedLayer;
        }

        public void DeleteLayer(Layer layer)
        {
            Layers.Remove(layer);
        }

        public Layer DoubleLayer(Layer layer)
        {
            var index = Layers.IndexOf(layer);
            var newLayer = new Layer(SelectedLayer.Name + "1", new WriteableBitmap(SelectedLayer.Bitmap), false);
            Layers.Insert(Layers.IndexOf(SelectedLayer), newLayer);
            return newLayer;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
