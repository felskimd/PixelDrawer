using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using System.Windows.Media.Imaging;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace PixelDrawer.ViewModel
{
    public class ProjectsVM: INotifyPropertyChanged
    {
        private Project? selectedProject;
        public Project SelectedProject 
        { 
            get => selectedProject; 
            set
            {
                selectedProject = value;
                var layersListBox = Application.Current.MainWindow.FindName("layersView") as ListBox;
                //layersListBox.ItemsSource = GetLayersView(selectedProject);
                //layersListBox.Items.Clear();
                var binding = new Binding();
                binding.Source = GetLayersView(selectedProject);
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                layersListBox.SetBinding(ListBox.ItemsSourceProperty, binding);
                if (value != null) SelectedLayer = selectedProject.SelectedLayer;
                OnPropertyChanged("SelectedProject");
            }
        }

        private Layer? selectedLayer;
        public Layer SelectedLayer
        {
            get => selectedLayer;
            set
            {
                selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        public LayersView SelectedProjectLayersView
        {
            get => LayersViews.Where(x => x.RelatedProject == SelectedProject).First();
        }

        private Projects Projects = Projects.Current;
        public ObservableCollection<Project> ProjectsList { get { return Projects.ProjectsList; } }
        private ObservableCollection<LayersView> LayersViews = new ObservableCollection<LayersView>();

        public void AddProjectLayersView(Project project)
        {
            LayersViews.Add(new LayersView(project));
        }

        public void RemoveProjectLayersView(Project project)
        {
            LayersViews.Remove(LayersViews.Where(x => x.RelatedProject == project).First());
        }

        public ObservableCollection<UIElement> GetLayersView(Project project)
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
            public ObservableCollection<Layer> RelatedLayers;
            public Project RelatedProject { get; }

            public LayersView(Project project)
            {
                RelatedProject = project;
                RelatedLayers = project.Layers;
                for (var i = 0; i < project.Layers.Count; i++) 
                {
                    AddNewLayer(project.Layers[i]);
                }
            }

            public StackPanel PrepareNewLayer(Layer newLayer)
            {
                var stackPanel = new StackPanel();
                stackPanel.Tag = newLayer;
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
                var moveUpImage = new Image();
                moveUpImage.Source = new BitmapImage(new Uri("/Icons/UpArrow.bmp", UriKind.Relative));
                moveUpButton.Content = moveUpImage;
                moveUpButton.Click += (s, e) =>
                {
                    MoveLayerUp(s as Button);
                };
                var moveDownButton = new Button();
                var moveDownImage = new Image();
                moveDownImage.Source = new BitmapImage(new Uri("/Icons/DownArrow.bmp", UriKind.Relative));
                moveDownButton.Content = moveDownImage;
                moveDownButton.Click += (s, e) =>
                {
                    MoveLayerDown(s as Button);
                };
                stackPanel.Children.Add(visibilityToggleButton);
                stackPanel.Children.Add(layerNameBox);
                stackPanel.Children.Add(moveUpButton);
                stackPanel.Children.Add(moveDownButton);
                return stackPanel;
            }

            public void AddNewLayer(Layer newLayer)
            {
                Views.Add(PrepareNewLayer(newLayer));
            }

            public void DoubleLayer(Layer newLayer, Layer oldLayer)
            {
                Views.Insert(Views.IndexOf(Views.Where(x => (x as StackPanel).Tag == oldLayer).First()), 
                    PrepareNewLayer(newLayer));
            }

            public void RemoveLayer(Layer layer)
            {
                Views.Remove(Views.Where(x => (x as StackPanel).Tag == layer).First());
            }

            private void MoveLayerUp(Button btn)
            {
                var stackPanel = btn.Parent as StackPanel;
                var layer = stackPanel.Tag as Layer;
                var index = RelatedLayers.IndexOf(layer);
                if (index < RelatedLayers.Count - 1)
                {
                    RelatedLayers.RemoveAt(index);
                    RelatedLayers.Insert(index + 1, layer);
                    Views.RemoveAt(index);
                    Views.Insert(index + 1, stackPanel);
                }
            }

            private void MoveLayerDown(Button btn)
            {
                var stackPanel = btn.Parent as StackPanel;
                var layer = stackPanel.Tag as Layer;
                var index = RelatedLayers.IndexOf(layer);
                if (index > 0)
                {
                    RelatedLayers.RemoveAt(index);
                    RelatedLayers.Insert(index - 1, layer);
                    Views.RemoveAt(index);
                    Views.Insert(index - 1, stackPanel);
                }
            }

            public ObservableCollection<UIElement> GetViews() => Views;
        }
    }
}
