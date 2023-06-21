using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace PixelDrawer.ViewModel
{
    public class ProjectsVM: INotifyPropertyChanged
    {
        private TestProject? selectedProject;
        public TestProject SelectedProject 
        { 
            get => selectedProject; 
            set
            {
                selectedProject = value;
                var layersListBox = Application.Current.MainWindow.FindName("layersView") as ListBox;
                layersListBox.ItemsSource = GetLayersView(selectedProject);
                //layersListBox.Items.Clear();
                if (value != null) SelectedLayer = selectedProject.SelectedLayer;
                OnPropertyChanged("SelectedProject");
            }
        }

        private TestLayer? selectedLayer;
        public TestLayer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        //public ObservableCollection<UIElement> SelectedProjectLayersView = new ObservableCollection<UIElement>();
        private TestProjects Projects = TestProjects.Current;
        public ObservableCollection<TestProject> ProjectsList { get { return Projects.ProjectsList; } }
        public ObservableCollection<LayersView> LayersViews = new ObservableCollection<LayersView>();

        public void AddProjectLayersView(TestProject project)
        {
            LayersViews.Add(new LayersView(project));
        }

        public void RemoveProjectLayersView(TestProject project)
        {
            LayersViews.Remove(LayersViews.Where(x => x.RelatedProject == project).First());
        }

        public ObservableCollection<UIElement> GetLayersView(TestProject project)
        {
            return LayersViews.Where(x => x.RelatedProject == project).First().GetViews();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public class LayersView
        {
            private ObservableCollection<UIElement> Views = new ObservableCollection<UIElement>();
            public ObservableCollection<TestLayer> RelatedLayers;
            public TestProject RelatedProject { get; }

            public LayersView(TestProject project)
            {
                RelatedProject = project;
                RelatedLayers = project.Layers;
                for (var i = 0; i < project.Layers.Count; i++) 
                {
                    //ToMethod
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    var visibilityToggleButton = new ToggleButton();
                    var visibilityBinding = new Binding();
                    visibilityBinding.Source = project.Layers[i];
                    visibilityBinding.Mode = BindingMode.TwoWay;
                    visibilityBinding.Path = new PropertyPath("IsVisible");
                    visibilityToggleButton.SetBinding(ToggleButton.IsCheckedProperty, visibilityBinding);
                    var layerNameBox = new TextBox();
                    var layerNameBinding = new Binding();
                    layerNameBinding.Source = project.Layers[i];
                    layerNameBinding.Mode = BindingMode.TwoWay;
                    layerNameBinding.Path = new PropertyPath("Name");
                    layerNameBox.SetBinding(TextBox.TextProperty, layerNameBinding);
                    var moveUpButton = new Button();
                    var moveDownButton = new Button();
                    stackPanel.Children.Add(visibilityToggleButton);
                    stackPanel.Children.Add(layerNameBox);
                    stackPanel.Children.Add(moveUpButton);
                    stackPanel.Children.Add(moveDownButton);
                    stackPanel.Tag = project.Layers[i];
                    //var eventTrigger = new Microsoft.Xaml.Behaviors.EventTrigger("");
                    //var commandBinding = new CommandBinding();
                    //commandBinding.Command = new RelayCommand();
                    //commandBinding.
                    //stackPanel.CommandBindings.Add(commandBinding);
                    Views.Add(stackPanel);
                }
            }

            public void AddNewLayer(TestLayer newLayer)
            {
                var stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                var visibilityToggleButton = new ToggleButton();
                var visibilityBinding = new Binding();
                visibilityBinding.Source = newLayer;
                visibilityBinding.Mode = BindingMode.TwoWay;
                visibilityBinding.Path = new PropertyPath("IsVisible");
                visibilityToggleButton.SetBinding(ToggleButton.IsCheckedProperty, visibilityBinding);
                var layerNameBox = new TextBox();
                var layerNameBinding = new Binding();
                layerNameBinding.Source = newLayer;
                layerNameBinding.Mode = BindingMode.TwoWay;
                layerNameBinding.Path = new PropertyPath("Name");
                layerNameBox.SetBinding(TextBox.TextProperty, layerNameBinding);
                var moveUpButton = new Button();
                var moveDownButton = new Button();
                stackPanel.Children.Add(visibilityToggleButton);
                stackPanel.Children.Add(layerNameBox);
                stackPanel.Children.Add(moveUpButton);
                stackPanel.Children.Add(moveDownButton);
                stackPanel.Tag = newLayer;
                Views.Add(stackPanel);
            }

            public ObservableCollection<UIElement> GetViews() => Views;
        }
    }
}
