using Microsoft.Win32;
using Microsoft.Xaml.Behaviors;
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PixelDrawer.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            Tabs = new ObservableCollection<TabItem>();
            Tools = new ObservableCollection<Tool>();
            SelectedToolPropertiesList = new ObservableCollection<UIElement>();
            Model.Tools.AddTools(Tools);
            selectedColor = Colors.White;
        }

        private TabItem? selectedTabItem;
        public TabItem? SelectedTabItem
        {
            get { return selectedTabItem; }
            set
            {
                selectedTabItem = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public ObservableCollection<TabItem> Tabs { get; set; }

        public ObservableCollection<Tool> Tools { get; set; }

        public ObservableCollection<UIElement> SelectedToolPropertiesList { get; set; }

        private Tool? selectedTool;
        public Tool? SelectedTool
        {
            get { return selectedTool; }
            set
            {
                selectedTool = value;
                if (SelectedToolPropertiesList.Count != 0)
                    SelectedToolPropertiesList.Clear();
                foreach (var toolProp in selectedTool.ToolProperties)
                    SelectedToolPropertiesList.Add(toolProp);
                OnPropertyChanged("SelectedTool");
            }
        }

        private Color selectedColor;
        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }

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

        #region Commands
        private RelayCommand? showOpenWindowCmd;
        public RelayCommand ShowOpenWindowCmd
        {
            get
            {
                return showOpenWindowCmd ??
                  (showOpenWindowCmd = new RelayCommand(obj =>
                  {
                      ShowOpenWindow();
                  }));
            }
        }

        private RelayCommand? showCreateWindowCmd;
        public RelayCommand ShowCreateWindowCmd
        {
            get
            {
                return showCreateWindowCmd ??
                  (showCreateWindowCmd = new RelayCommand(obj =>
                  {
                      ShowCreateWindow();
                  }));
            }
        }

        private RelayCommand? tabControlCloseCmd;
        public RelayCommand TabControlCloseCmd
        {
            get
            {
                return tabControlCloseCmd ??
                  (tabControlCloseCmd = new RelayCommand(obj =>
                  {
                      Tabs.Remove(SelectedTabItem);
                  }));
            }
        }

        private RelayCommand? changeSelectedToolCmd;
        public RelayCommand ChangeSelectedToolCmd
        {
            get
            {
                return changeSelectedToolCmd ??
                  (changeSelectedToolCmd = new RelayCommand(obj =>
                  {
                      ChangeSelectedTool(obj as string);
                  }));
            }
        }

        private RelayCommand? drawMouseMoveCmd;
        public RelayCommand DrawMouseMoveCmd
        {
            get
            {
                return drawMouseMoveCmd ??
                  (drawMouseMoveCmd = new RelayCommand(obj =>
                  {
                      DrawMouseMove(obj as MouseEventArgs);
                  }));
            }
        }

        private RelayCommand? mouseWheelCmd;
        public RelayCommand MouseWheelCmd
        {
            get
            {
                return mouseWheelCmd ??
                  (mouseWheelCmd = new RelayCommand(obj =>
                  {
                      MouseWheel(obj as MouseWheelEventArgs);
                  }));
            }
        }
        #endregion
        private void MouseWheel(MouseWheelEventArgs e)
        {
            var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
            var tabItem = tabControl.SelectedItem as TabItem;
            if (tabItem.Scale < 0.3 && e.Delta < 0)
            {
                return;
            }
            ZoomCenterPoint = new Point(CurrentPoint.X, CurrentPoint.Y);
            tabItem.Scale += (double)e.Delta / 500;
        }

        private void DrawMouseMove(MouseEventArgs e)
        {
            var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
            var img = GetImageFromTabControl(tabControl);
            OldPoint = CurrentPoint;
            CurrentPoint = Application.Current.MainWindow.TranslatePoint(
                e.GetPosition(Application.Current.MainWindow), img);
            if (e.LeftButton == MouseButtonState.Pressed && selectedTool != null)
            {
                selectedTool.Execute(selectedTabItem.Content, OldPoint, CurrentPoint, SelectedColor);
            }
        }

        public void AddPicture(string name, WriteableBitmap bmp)
        {
            var newTab = new TabItem(name, bmp);
            Tabs.Add(newTab);
            SelectedTabItem = newTab;
        }

        private void ShowCreateWindow()
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Owner = Application.Current.MainWindow;
            createWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createWindow.ShowDialog();
        }

        private void ShowOpenWindow()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|PixelDrawer files (*.pdpr)|*.pdpr";
            openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                AddPicture(
                    openFileDialog.FileName,
                    new WriteableBitmap(bitmap));
            }
        }

        private void ChangeSelectedTool(string tool)
        {
            SelectedTool = Tools.Where(x => x.Name == tool).First();
        }

        private Image GetImageFromTabControl(TabControl tabControl)
        {
            return VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(
                VisualTreeHelper.GetChild(
                    VisualTreeHelper.GetChild(
                        VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(tabControl, 0), 0), 0), 0), 0), 0), 1), 0), 0) as Image;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public sealed class TabItem : DependencyObject, INotifyPropertyChanged
    {
        public string Header { get; set; }
        private WriteableBitmap content;
        public WriteableBitmap Content { get { return content; }
            set
            {
                content = value;
                OnPropertyChanged("Content");
            }
        }
        private double scale;
        public double Scale { get { return scale; }
            set
            {
                scale = value;
                OnPropertyChanged("Scale");
            } 
        }

        public TabItem(string name, WriteableBitmap bmp)
        {
            Header = name;
            Content = bmp;
            scale = 1.0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
