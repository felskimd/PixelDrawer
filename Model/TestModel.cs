using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Reflection;
using System.Windows.Controls;

namespace PixelDrawer.Model
{
    class TestModel
    {
    }

    public interface TestTool : INotifyPropertyChanged
    {
        public string ToolName { get; }
        public int ToolId { get; }
    }

    public class TestTools
    {
        private static ObservableCollection<TestTool> tools;

        static TestTools()
        {
            tools = new ObservableCollection<TestTool>()
            {
                new TestPencilTool(),
                new TestFillTool(),
                new TestPipetteTool(),
                new TestSelectionTool(),
                new TestEraserTool(),
            };
        }

        public static void AddTools(ObservableCollection<TestTool> Tools)
        {
            foreach (TestTool tool in tools)
            {
                Tools.Add(tool);
            }

        }

        public static ObservableCollection<TestTool> GetTools()
        {
            return tools;
        }
    }

    public class TestPencilTool : TestTool
    {
        public string ToolName { get { return "Pencil"; } }
        public int ToolId { get { return 0; } }

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

        public TestPencilTool()
        {
            size = 1;
        }

        public void Execute(WriteableBitmap bmp, System.Windows.Point currentPoint, System.Windows.Media.Color color)
        {
            bmp.FillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, color);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestFillTool : TestTool
    {
        public string ToolName { get { return "Fill"; } }
        public int ToolId { get { return 1; } }

        public void Execute(WriteableBitmap bmp, System.Windows.Media.Color color)
        {
            bmp.Clear(color);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestPipetteTool : TestTool
    {
        public string ToolName { get { return "Pipette"; } }
        public int ToolId { get { return 2; } }

        public Color Execute(WriteableBitmap bmp, System.Windows.Point currentPoint)
        {
            return bmp.GetPixel((int)currentPoint.X, (int)currentPoint.Y);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestSelectionTool : TestTool
    {
        public string ToolName { get { return "Selection"; } }
        public int ToolId { get { return 3; } }

        public Rect Execute(Point firstPoint, Point secondPoint)
        {
            return new Rect(firstPoint, secondPoint);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestEraserTool : TestTool
    {
        public string ToolName { get { return "Eraser"; } }
        public int ToolId { get { return 4; } }

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

        public TestEraserTool()
        {
            size = 1;
        }

        public void Execute(WriteableBitmap bmp, System.Windows.Point currentPoint)
        {
            bmp.FillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, Colors.Transparent);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestProject: INotifyPropertyChanged
    {
        public string Name { get; }
        public ObservableCollection<TestLayer> Layers { get; private set; }
        public ObservableCollection<Image> Images { get; private set; }

        private int newLayersCount = 1;
        private int NewLayersCount { get
            {
                newLayersCount++;
                return newLayersCount;
            }
            set 
            { 
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private TestLayer selectedLayer;
        public TestLayer SelectedLayer { get { return selectedLayer; }
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

        public TestProject(string name, Color backgroundColor, int width, int height)
        {
            Name = name;
            Layers = new ObservableCollection<TestLayer>
            {
                new TestLayer("Background", width, height, backgroundColor)
            };
            var image = new Image();
            image.Source = Layers.First().Bitmap;
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            Images = new ObservableCollection<Image>
            {
                image
            };
            Width = width;
            Height = height;
            SelectedLayer = Layers[0];
        }

        public TestProject(string name, WriteableBitmap bmp)
        {
            Name = name;
            Layers = new ObservableCollection<TestLayer>
            {
                new TestLayer("Background", bmp)
            };
            var image = new Image();
            image.Source = Layers.First().Bitmap;
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            Images = new ObservableCollection<Image>
            {
                image
            };
            Width = (int)bmp.Width;
            Height = (int)bmp.Height;
            SelectedLayer = Layers[0];
        }

        public TestLayer AddLayer()
        {
            Layers.Add(new TestLayer(NewLayersCount.ToString(), Width, Height));
            SelectedLayer = Layers.Last();
            return SelectedLayer;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestLayer: INotifyPropertyChanged
    {
        private string name;
        public string Name { get { return name; }
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
        public bool IsVisible { get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
                OnPropertyChanged("Visibility");
            }
        }

        public Visibility Visibility { get { return isVisible ? Visibility.Visible : Visibility.Collapsed; } 
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

        public TestLayer(string name, int width, int height)
        {
            Name = name;
            Bitmap = BitmapFactory.New(width, height);
        }

        public TestLayer(string name, int width, int height, Color color)
        {
            Name = name;
            Bitmap = BitmapFactory.New(width, height);
            Bitmap.Clear(color);
        }

        public TestLayer(string name, WriteableBitmap bmp)
        {
            Name = name;
            Bitmap = bmp;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestProjects: INotifyPropertyChanged
    {
        private static readonly TestProjects instance = new TestProjects();
        public static TestProjects Current => instance;

        public readonly ObservableCollection<TestProject> ProjectsList;

        public TestProjects()
        {
            ProjectsList = new ObservableCollection<TestProject>();
        }

        public void AddProject(string title, int width, int height, Color backgroundColor)
        {
            ProjectsList.Add(new TestProject(title, backgroundColor, width, height));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class TestFileManagement
    {

    }
}
