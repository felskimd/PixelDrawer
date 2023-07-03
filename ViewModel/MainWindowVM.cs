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
using System.Windows.Shapes;
using Microsoft.Xaml.Behaviors.Layout;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace PixelDrawer.ViewModel
{
    class MainWindowVM
    {
        public ToolsVM Tools { get; }
        public ProjectsVM Projects { get; }
        public ColorsVM Colors { get; }
        public PointsVM Points { get; }
        public Image CurrentImage { get; set; }
        public Canvas CurrentCanvas { get; set; }
        private MouseDragElementBehavior mouseDragElementBehavior = new MouseDragElementBehavior();

        public MainWindowVM()
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
                      TabControlClose();
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
                      ChangeSelectedTool(obj as int?);
                  }));
            }
        }

        private RelayCommand? changeSelectedLayerCmd;
        public RelayCommand ChangeSelectedLayerCmd
        {
            get
            {
                return changeSelectedLayerCmd ??
                  (changeSelectedLayerCmd = new RelayCommand(obj =>
                  {
                      ChangeSelectedLayer(obj as SelectionChangedEventArgs);
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

        private RelayCommand? drawMouseClickCmd;
        public RelayCommand DrawMouseClickCmd
        {
            get
            {
                return drawMouseClickCmd ??
                  (drawMouseClickCmd = new RelayCommand(obj =>
                  {
                      DrawMouseClick(obj as MouseEventArgs);
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

        private RelayCommand? addLayerCmd;
        public RelayCommand AddLayerCmd
        {
            get
            {
                return addLayerCmd ??
                    (addLayerCmd = new RelayCommand(obj =>
                    {
                        AddLayer();
                    }, obj =>
                    {
                        if (Projects.SelectedProject != null) return true; 
                        else return false;
                    }
                    ));
            }
        }

        private RelayCommand? undoCmd;
        public RelayCommand UndoCmd
        {
            get
            {
                return undoCmd ??
                  (undoCmd = new RelayCommand(obj =>
                  {
                      Undo();
                  }, obj =>
                  {
                      return false;
                  }
                  ));
            }
        }

        private RelayCommand? redoCmd;
        public RelayCommand RedoCmd
        {
            get
            {
                return redoCmd ??
                  (redoCmd = new RelayCommand(obj =>
                  {
                      Redo();
                  }, obj =>
                  {
                      return false;
                  }
                  ));
            }
        }

        private RelayCommand? saveAsCmd;
        public RelayCommand SaveAsCmd
        {
            get
            {
                return saveAsCmd ??
                  (saveAsCmd = new RelayCommand(obj =>
                  {
                      SaveAs();
                  }));
            }
        }

        private RelayCommand? saveCmd;
        public RelayCommand SaveCmd
        {
            get
            {
                return saveCmd ??
                  (saveCmd = new RelayCommand(obj =>
                  {
                      SaveProject();
                  }));
            }
        }

        private RelayCommand? mouseEnterCmd;
        public RelayCommand MouseEnterCmd
        {
            get
            {
                return mouseEnterCmd ??
                  (mouseEnterCmd = new RelayCommand(obj =>
                  {
                      var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
                      CurrentImage = GetImageFromTabControl(tabControl);
                  }));
            }
        }

        private RelayCommand? doubleLayerCmd;
        public RelayCommand DoubleLayerCmd
        {
            get
            {
                return doubleLayerCmd ??
                  (doubleLayerCmd = new RelayCommand(obj =>
                  {
                      var newLayer = Projects.SelectedProject.DoubleLayer(Projects.SelectedProject.SelectedLayer);
                      Projects.SelectedProjectLayersView.DoubleLayer(newLayer, Projects.SelectedProject.SelectedLayer);
                  }));
            }
        }

        private RelayCommand? deleteLayerCmd;
        public RelayCommand DeleteLayerCmd
        {
            get
            {
                return deleteLayerCmd ??
                  (deleteLayerCmd = new RelayCommand(obj =>
                  {
                      Projects.SelectedProject.DeleteLayer(Projects.SelectedProject.SelectedLayer);
                      Projects.SelectedProjectLayersView.RemoveLayer(Projects.SelectedProject.SelectedLayer);
                  }));
            }
        }

        private RelayCommand? spaceKeyDownCmd;
        public RelayCommand SpaceKeyDownCmd
        {
            get
            {
                return spaceKeyDownCmd ??
                    (spaceKeyDownCmd = new RelayCommand(obj =>
                    {
                        var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
                        var border = GetBorderFromTabControl(tabControl);
                        border.ForceCursor = true;
                        border.Cursor = Cursors.Hand;
                        if (!Interaction.GetBehaviors(border).Contains(mouseDragElementBehavior))
                            Interaction.GetBehaviors(border).Add(mouseDragElementBehavior);
                    }));
            }
        }

        private RelayCommand? spaceKeyUpCmd;
        public RelayCommand SpaceKeyUpCmd
        {
            get
            {
                return spaceKeyUpCmd ??
                    (spaceKeyUpCmd = new RelayCommand(obj =>
                    {
                        var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
                        var border = GetBorderFromTabControl(tabControl);
                        border.ForceCursor = false;
                        border.Cursor = Cursors.Cross;
                        Interaction.GetBehaviors(border).Remove(mouseDragElementBehavior);
                    }));
            }
        }
        #endregion

        private void MouseWheel(MouseWheelEventArgs e)
        {
            if (Projects.SelectedProject.Scale < 0.3 && e.Delta < 0)
            {
                return;
            }
            //Points.ZoomCenterPoint = new Point(Points.CurrentPoint.X, Points.CurrentPoint.Y);
            Points.ZoomCenterPoint = e.GetPosition(VisualTreeHelperEx.FindDescendantByName(Application.Current.MainWindow, "grid") as Grid);
            if (Projects.SelectedProject.Scale < 7)
            {
                Projects.SelectedProject.Scale += (double)e.Delta / 500;
            }
            else if (Projects.SelectedProject.Scale < 14)
            {
                Projects.SelectedProject.Scale += (double)e.Delta / 250;
            }
            else
            {
                Projects.SelectedProject.Scale += (double)e.Delta / 125;
            }
            Projects.SelectedProject.Scale = Math.Round(Projects.SelectedProject.Scale, 2);
        }

        private void DrawMouseMove(MouseEventArgs e)
        {
            Points.OldPoint = Points.CurrentPoint;
            Points.CurrentPoint = Application.Current.MainWindow.TranslatePoint(
                e.GetPosition(Application.Current.MainWindow), CurrentImage);
            if (e.LeftButton == MouseButtonState.Pressed && Tools.SelectedTool != null && !Keyboard.IsKeyDown(Key.Space))
            {
                switch (Tools.SelectedTool.ToolId)
                {
                    case 0:
                        if (Projects.SelectedLayer != null)
                        {
                            var tool0 = Tools.SelectedTool as PencilTool;
                            tool0.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint, Colors.SelectedColor);
                        }
                        break;
                    case 1:
                        var tool1 = Tools.SelectedTool as FillTool;
                        tool1.Execute(Projects.SelectedLayer.Bitmap, Colors.SelectedColor);
                        break; 
                    case 2:
                        var tool2 = Tools.SelectedTool as PipetteTool;
                        Colors.SelectedColor = tool2.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                    case 3:
                        var tool3 = Tools.SelectedTool as SelectionTool;
                        //todo
                        break;
                    case 4:
                        var tool4 = Tools.SelectedTool as EraserTool;
                        tool4.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                }
            }
        }

        private void DrawMouseClick(MouseEventArgs e)
        {
            if (Tools.SelectedTool != null && !Keyboard.IsKeyDown(Key.Space))
            {
                switch (Tools.SelectedTool.ToolId)
                {
                    case 0:
                        if (Projects.SelectedLayer != null)
                        {
                            var tool0 = Tools.SelectedTool as PencilTool;
                            tool0.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint, Colors.SelectedColor);
                        }
                        break;
                    case 1:
                        var tool1 = Tools.SelectedTool as FillTool;
                        tool1.Execute(Projects.SelectedLayer.Bitmap, Colors.SelectedColor);
                        break;
                    case 2:
                        var tool2 = Tools.SelectedTool as PipetteTool;
                        Colors.SelectedColor = tool2.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                    case 3:
                        var tool3 = Tools.SelectedTool as SelectionTool;
                        //todo
                        break;
                    case 4:
                        var tool4 = Tools.SelectedTool as EraserTool;
                        tool4.Execute(Projects.SelectedLayer.Bitmap, Points.CurrentPoint);
                        break;
                }
            }
        }

        public void AddProject(string name, WriteableBitmap bmp)
        {
            var newProject = new Project(name, bmp);
            Projects.ProjectsList.Add(newProject);
            Projects.AddProjectLayersView(newProject);
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
            openFileDialog.DefaultExt = ".pdpr";
            openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName.Split(".").Last() == "pdpr")
                {
                    var newProject = FileSaveLoad.OpenProject(openFileDialog.FileName);
                    Model.Projects.Current.AddProject(newProject);
                    Projects.AddProjectLayersView(newProject);
                    return;
                }
                BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                AddProject(
                    openFileDialog.FileName.Split("\\").Last(),
                    new WriteableBitmap(bitmap));
            }
        }

        private void SaveAs()
        {
            var tabControl = Application.Current.MainWindow.FindName("projects") as TabControl;
            var canvas = GetCanvasFromTabControl(tabControl);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = ".png|*.png|.jpeg|*.jpeg|.bmp|*.bmp|.gif|*.gif";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.FileName = Projects.SelectedProject.Name;
            if (saveFileDialog.ShowDialog() == true)
            {
                FileSaveLoad.SaveFileAs(saveFileDialog.FileName, Projects.SelectedProject, canvas);
            }
        }

        private void SaveProject()
        {
            FileSaveLoad.SaveProject(Projects.SelectedProject);
        }

        private void ChangeSelectedTool(int? toolId)
        {
            Tools.SelectedTool = Tools.Tools[toolId ?? 0];
        }

        public void ChangeSelectedLayer(SelectionChangedEventArgs args)
        {
            StackPanel stackPanel = (Application.Current.MainWindow.FindName("layersView") as ListBox).SelectedItem as StackPanel;
            if (stackPanel != null)
            {
                Projects.SelectedProject.SelectedLayer = stackPanel.Tag as Layer;
                Projects.SelectedLayer = stackPanel.Tag as Layer;
            }
        }

        private void Undo()
        {
        }

        private void Redo()
        {
        }

        private void AddLayer()
        {
            var newLayer = Projects.SelectedProject.AddLayer();
            //Projects.LayersViews.Where(x => x.RelatedProject == Projects.SelectedProject).First().AddNewLayer(newLayer);
            Projects.SelectedProjectLayersView.AddNewLayer(newLayer);

        }

        private void TabControlClose()
        {
            Projects.RemoveProjectLayersView(Projects.SelectedProject);
            Projects.ProjectsList.Remove(Projects.SelectedProject);
        }

        private Image GetImageFromTabControl(TabControl tabControl)
        {
            return VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(
                    VisualTreeHelper.GetChild(
                        VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(tabControl, 0), 0), 0), 0), 0), 0), 1), 0), 0), 0), 0), 0), 0), 0) as Image;
        }

        private Canvas GetCanvasFromTabControl(TabControl tabControl)
        {
            return VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(
                    VisualTreeHelper.GetChild(
                        VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(tabControl, 0), 0), 0), 0), 0), 0), 1), 0), 0), 0), 0), 0) as Canvas;
        }

        private Border GetBorderFromTabControl(TabControl tabControl)
        {
            return VisualTreeHelper.GetChild(
                        VisualTreeHelper.GetChild(
                            VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(
                                VisualTreeHelper.GetChild(
                                    VisualTreeHelper.GetChild(
                                        VisualTreeHelper.GetChild(
                                            VisualTreeHelper.GetChild(tabControl, 0), 0), 0), 0), 0), 0), 1), 0) as Border;
        }
    }
}
