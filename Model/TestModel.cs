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

    public class TestProject
    {
        public readonly string Name;
        public ObservableCollection<TestLayer> Layers { get; private set; }

        private Stack<ObservableCollection<TestLayer>> projectUndoStack;
        private Stack<ObservableCollection<TestLayer>> projectRedoStack;

        public TestProject(string name, Color backgroundColor, int width, int height)
        {
            Name = name;
            Layers = new ObservableCollection<TestLayer>
            {
                new TestLayer("Background", width, height, backgroundColor)
            };
        }

        public TestProject(string name, WriteableBitmap bmp)
        {
            Name = name;
            Layers = new ObservableCollection<TestLayer>
            {
                new TestLayer("Background", bmp.Clone())
            };
        }

        public void Undo()
        {
            if (CanUndo())
            {
                var temp = projectUndoStack.Pop();
                Layers = temp;
                projectRedoStack.Push(temp);
            }
        }

        public void Redo()
        {
            if (CanRedo())
            {
                var temp = projectRedoStack.Pop();
                Layers = temp;
                projectUndoStack.Push(temp);
            }
        }

        public bool CanRedo()
        {
            return projectRedoStack.Count == 0 ? false : true;
        }

        public bool CanUndo()
        {
            return projectUndoStack.Count == 0 ? false : true;
        }
    }

    public class TestLayer
    {
        public readonly string Name;
        public readonly WriteableBitmap Bitmap;

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
    }

    public class TestProjects: INotifyPropertyChanged
    {
        //singleton
        private static readonly TestProjects instance = new TestProjects();
        public static TestProjects Current => instance;

        public readonly ObservableCollection<TestProject> ProjectsList;

        public TestProjects()
        {
            ProjectsList = new ObservableCollection<TestProject>();
        }

        //Methods
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
