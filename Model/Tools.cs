using PixelDrawer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit;

namespace PixelDrawer.Model
{
    public class Tools
    {
        private static ObservableCollection<Tool> tools;

        static Tools()
        {
            tools = new ObservableCollection<Tool>()
            {
                new PencilTool(),
                new FillTool(),
                new PipetteTool(),
                new SelectionTool(),
                new EraserTool(),
            };
        }

        public static void AddTools(ObservableCollection<Tool> Tools)
        {
            foreach (Tool tool in tools) 
            { 
                Tools.Add(tool);
            }
            
        }

        public static ObservableCollection<Tool> GetTools()
        {
            return tools;
        }
    }

    public abstract class Tool: INotifyPropertyChanged
    {
        public abstract string Name { get; }
        public abstract List<UIElement> ToolProperties { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public abstract void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color);  
    }

    public class PencilTool : Tool
    {
        public override string Name { get; }

        public override List<UIElement> ToolProperties { get; }

        private int size;
        public int Size { get { return size; }
            set
            {
                if (value <= 0) size = 1;
                else size = value;
                OnPropertyChanged("Size");
            }
        }
        public PencilTool()
        {
            Name = "Pencil";
            Size = 1;
            ToolProperties = new List<UIElement>();
            InitialiseToolProperties();
        }

        public override void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color)
        {
            //bmp.DrawLineAa((int)oldPoint.X, (int)oldPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, color, Size);
            bmp.FillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, color);
        }

        private void InitialiseToolProperties()
        {
            var sizePropertyName = new TextBlock();
            sizePropertyName.Text = "Size";
            var sizeSlider = new Slider();
            sizeSlider.Value = 5;
            sizeSlider.Minimum = 1;
            sizeSlider.Maximum = 200;
            sizeSlider.SmallChange = 1;
            sizeSlider.LargeChange = 10;
            var sizeText = new IntegerUpDown();
            sizeText.Minimum = 1;
            sizeText.Maximum = 200;
            var binding = new Binding("Size");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sizeSlider.SetBinding(Slider.ValueProperty, binding);
            sizeText.SetBinding(IntegerUpDown.TextProperty, binding);
            ToolProperties.Add(sizePropertyName);
            ToolProperties.Add(sizeSlider);
            ToolProperties.Add(sizeText);
        }
    }

    public class FillTool : Tool
    {
        public override string Name { get; }

        public override List<UIElement> ToolProperties => new List<UIElement>();

        public FillTool()
        {
            Name = "Fill";
        }

        public override void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color)
        {
            bmp.Clear(color);
        }
    }

    public class PipetteTool : Tool
    {
        public override string Name { get; }

        public override List<UIElement> ToolProperties => new List<UIElement>();

        public PipetteTool()
        {
            Name = "Pipette";
        }

        public override void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color)
        {
            var vm = Application.Current.MainWindow.DataContext as MainWindowVM;
            vm.SelectedColor = bmp.GetPixel((int) currentPoint.X, (int)currentPoint.Y);
        }
    }

    public class SelectionTool : Tool
    {
        public override string Name { get; }

        public override List<UIElement> ToolProperties => throw new NotImplementedException();

        public SelectionTool()
        {
            Name = "Selection";
        }

        public override void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color)
        {
            throw new NotImplementedException();
        }
    }

    public class EraserTool : Tool
    {
        public override string Name { get; }

        public override List<UIElement> ToolProperties { get; }

        private int size;
        public int Size
        {
            get { return size; }
            set
            {
                if (value <= 0) size = 1;
                else size = value;
                OnPropertyChanged("Size");
            }
        }
        public EraserTool()
        {
            Name = "Eraser";
            Size = 1;
            ToolProperties = new List<UIElement>();
            InitialiseToolProperties();
        }

        public override void Execute(WriteableBitmap bmp, Point oldPoint, Point currentPoint, System.Windows.Media.Color color)
        {
            //bmp.DrawLineAa((int)oldPoint.X, (int)oldPoint.Y, (int)currentPoint.X, (int)currentPoint.Y, color, Size);
            bmp.FillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, Colors.Transparent);
        }

        private void InitialiseToolProperties()
        {
            var sizePropertyName = new TextBlock();
            sizePropertyName.Text = "Size";
            var sizeSlider = new Slider();
            sizeSlider.Value = 5;
            sizeSlider.Minimum = 1;
            sizeSlider.Maximum = 200;
            sizeSlider.SmallChange = 1;
            sizeSlider.LargeChange = 10;
            var sizeText = new IntegerUpDown();
            sizeText.Minimum = 1;
            sizeText.Maximum = 200;
            var binding = new Binding("Size");
            binding.Source = this;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sizeSlider.SetBinding(Slider.ValueProperty, binding);
            sizeText.SetBinding(IntegerUpDown.TextProperty, binding);
            ToolProperties.Add(sizePropertyName);
            ToolProperties.Add(sizeSlider);
            ToolProperties.Add(sizeText);
        }
    }
}
