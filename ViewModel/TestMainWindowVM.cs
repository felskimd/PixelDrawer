using Microsoft.Win32;
using PixelDrawer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace PixelDrawer.ViewModel
{
    class TestMainWindowVM
    {
        public ToolsVM Tools { get; }
        public ProjectsVM Projects { get; }
        public ColorsVM Colors { get; }
        public PointsVM Points { get; }

        public TestMainWindowVM()
        {
            Tools = new ToolsVM();
            Projects = new ProjectsVM();
            Colors = new ColorsVM();
            Points = new PointsVM();
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
                      Projects.ProjectsList.Remove(Projects.SelectedProject);
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
            Points.ZoomCenterPoint = new Point(Points.CurrentPoint.X, Points.CurrentPoint.Y);
            tabItem.Scale += (double)e.Delta / 500;
        }

        private void DrawMouseMove(MouseEventArgs e)
        {
            var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
            var img = GetImageFromTabControl(tabControl);
            Points.OldPoint = Points.CurrentPoint;
            Points.CurrentPoint = Application.Current.MainWindow.TranslatePoint(
                e.GetPosition(Application.Current.MainWindow), img);
            if (e.LeftButton == MouseButtonState.Pressed && Tools.SelectedTool != null)
            {
                switch (Tools.SelectedTool.ToolId)
                {
                    case 0:
                        var tool0 = Tools.SelectedTool as TestPencilTool;
                        Projects.SelectedProject.PushToUndoStack(Projects.SelectedProject.Layers);
                        tool0.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint, Colors.SelectedColor);
                        break;
                    case 1:
                        var tool1 = Tools.SelectedTool as TestFillTool;
                        tool1.Execute(Projects.SelectedLayer.Bitmap, Colors.SelectedColor);
                        break; 
                    case 2:
                        var tool2 = Tools.SelectedTool as TestPipetteTool;
                        tool2.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                    case 3:
                        var tool3 = Tools.SelectedTool as TestSelectionTool;
                        //todo
                        break;
                    case 4:
                        var tool4 = Tools.SelectedTool as TestEraserTool;
                        Projects.SelectedProject.PushToUndoStack(Projects.SelectedProject.Layers);
                        tool4.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                }
            }
        }

        public void AddPicture(string name, WriteableBitmap bmp)
        {
            var newProject = new TestProject(name, bmp);
            Projects.ProjectsList.Add(newProject);
            Projects.SelectedProject = newProject;
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
                    openFileDialog.Title,
                    new WriteableBitmap(bitmap));
            }
        }

        private void ChangeSelectedTool(string tool)
        {
            Tools.SelectedTool = Tools.Tools.Where(x => x.ToolName == tool).First();
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
    }
}
