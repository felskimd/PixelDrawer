using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;
using System.Windows.Shapes;

namespace PixelDrawer.ViewModel
{
    public class ToolsVM : INotifyPropertyChanged
    {
        public Dictionary<Tool, ToolProperties> ToolProperties { get; }

        private Tool? selectedTool;
        public Tool SelectedTool
        {
            get => selectedTool;
            set
            {
                selectedTool = value;
                SelectedToolProperties = ToolProperties[selectedTool];
                var mainWindowVM = Application.Current.MainWindow.DataContext as MainWindowVM;
                OnPropertyChanged("SelectedTool");
            }
        }

        private ToolProperties? selectedToolProperties;
        public ToolProperties SelectedToolProperties
        {
            get => selectedToolProperties; 
            set
            {
                selectedToolProperties = value;
                OnPropertyChanged("SelectedToolProperties");
            }
        }

        public ObservableCollection<Tool> Tools { get; }

        public ToolsVM()
        {
            Tools = Model.Tools.GetTools();
            ToolProperties = new Dictionary<Tool, ToolProperties>
            {
                { Tools[0], new PencilProperties() },
                { Tools[1], new FillProperties() },
                { Tools[2], new PipetteProperties() },
                { Tools[3], new SelectionProperties() },
                { Tools[4], new EraserProperties() }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public interface ToolProperties
    {
        public abstract ObservableCollection<UIElement> PropertiesView { get; }
        public abstract Tool linkedTool { get; }
    }

    internal class PencilProperties : ToolProperties
    {
        public ObservableCollection<UIElement> PropertiesView { get; }

        public Tool linkedTool => Tools.GetTools()[0];

        public PencilProperties()
        {
            PropertiesView = new ObservableCollection<UIElement>();
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
            binding.Source = linkedTool;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sizeSlider.SetBinding(Slider.ValueProperty, binding);
            sizeText.SetBinding(IntegerUpDown.TextProperty, binding);
            PropertiesView.Add(sizePropertyName);
            PropertiesView.Add(sizeSlider);
            PropertiesView.Add(sizeText);
        }
    }

    internal class FillProperties : ToolProperties
    {
        public ObservableCollection<UIElement> PropertiesView => new ObservableCollection<UIElement>();

        public Tool linkedTool => Tools.GetTools()[1];
    }

    internal class PipetteProperties : ToolProperties
    {
        public ObservableCollection<UIElement> PropertiesView => new ObservableCollection<UIElement>();

        public Tool linkedTool => Tools.GetTools()[2];
    }

    internal class SelectionProperties : ToolProperties
    {
        public ObservableCollection<UIElement> PropertiesView => new ObservableCollection<UIElement>();

        public Tool linkedTool => Tools.GetTools()[3];
    }

    internal class EraserProperties : ToolProperties
    {
        public ObservableCollection<UIElement> PropertiesView { get; }

        public Tool linkedTool => Tools.GetTools()[4];

        public EraserProperties()
        {
            PropertiesView = new ObservableCollection<UIElement>();
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
            binding.Source = linkedTool;
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sizeSlider.SetBinding(Slider.ValueProperty, binding);
            sizeText.SetBinding(IntegerUpDown.TextProperty, binding);
            PropertiesView.Add(sizePropertyName);
            PropertiesView.Add(sizeSlider);
            PropertiesView.Add(sizeText);
        }
    }
}
