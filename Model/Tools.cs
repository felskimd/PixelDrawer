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
using System.Drawing;

namespace PixelDrawer.Model
{
    public interface Tool : INotifyPropertyChanged
    {
        public string ToolName { get; }
        public int ToolId { get; }
    }

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

        public static ObservableCollection<Tool> GetTools()
        {
            return tools;
        }
    }

    public class PencilTool : Tool
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

        public PencilTool()
        {
            size = 1;
        }

        public void Execute(WriteableBitmap bmp, 
            Point currentPoint, 
            Point? previousPoint, 
            Point? prePreviousPoint, 
            Point? prePrePreviousPoint, 
            System.Windows.Media.Color color)
        {
            bmp.MyDrawCircle((int)currentPoint.X, (int)currentPoint.Y, Size, color);
            if (previousPoint.HasValue)
            {
                //bmp.DrawLineAa((int)previousPoint.Value.X, (int)previousPoint.Value.Y, (int)currentPoint.X, (int)currentPoint.Y, color, Size * 2);
                bmp.MyDrawLine(previousPoint.Value, currentPoint, Size, color);
            }
        }

        public void ExecuteInterpolated(WriteableBitmap bmp,
            Point currentPoint,
            Point? point1,
            Point? point2,
            Point? point3,
            System.Windows.Media.Color color)
        {
            //refactor
            if (!point1.HasValue)
            {
                bmp.MyDrawCircle((int)currentPoint.X, (int)currentPoint.Y, Size, color);
            }
            else if (!point2.HasValue)
            {
                //bmp.DrawLineAa((int)currentPoint.X, (int)currentPoint.Y, 
                //    (int)point1.Value.X, (int)point1.Value.Y, color, Size * 2);
                bmp.DrawLineAa((int)point1.Value.X, (int)point1.Value.Y, (int)currentPoint.X, (int)currentPoint.Y, color, Size * 2);
                bmp.MyDrawCircle((int)currentPoint.X, (int)currentPoint.Y, Size, color);
            }
            else if (!point3.HasValue)
            {
                var dx = point1.Value.X - point2.Value.X;
                var dy = point1.Value.Y - point2.Value.Y;
                var prevPoint = new Point(point2.Value.X - dx, point2.Value.Y - dy);
                var tempPoint = new Point(point2.Value.X, point2.Value.Y);
                for (double i = 0.1; i < 1.0; i += 0.1)
                {
                    var interpolatedPoint = DrawingInterpolation.CatmullRomInterpolation2d(currentPoint, point1 ?? new Point(), point2 ?? new Point(), prevPoint, i);
                    bmp.DrawLineAa((int)tempPoint.X, (int)tempPoint.Y, (int)interpolatedPoint.X, (int)interpolatedPoint.Y, color, Size * 2);
                    bmp.MyDrawCircle((int)interpolatedPoint.X, (int)interpolatedPoint.Y, Size, color);
                    tempPoint = interpolatedPoint;
                }
            }
            else
            {
                var tempPoint = new Point(point2.Value.X, point2.Value.Y);
                for (double i = 0.1; i < 1.0; i += 0.1)
                {
                    var interpolatedPoint = DrawingInterpolation.CatmullRomInterpolation2d(currentPoint, point1 ?? new Point(), point2 ?? new Point(), point3 ?? new Point(), i);
                    bmp.DrawLineAa((int)tempPoint.X, (int)tempPoint.Y, (int)interpolatedPoint.X, (int)interpolatedPoint.Y, color, Size * 2);
                    bmp.MyDrawCircle((int)interpolatedPoint.X, (int)interpolatedPoint.Y, Size, color);
                    tempPoint = interpolatedPoint;
                }
            }
        }

        public void Execute(WriteableBitmap bmp,
            Point currentPoint,
            System.Windows.Media.Color color)
        {
            bmp.MyDrawCircle((int)currentPoint.X, (int)currentPoint.Y, Size, color);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class FillTool : Tool
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

    public class PipetteTool : Tool
    {
        public string ToolName { get { return "Pipette"; } }
        public int ToolId { get { return 2; } }

        public System.Windows.Media.Color Execute(WriteableBitmap bmp, Point currentPoint)
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

    public class SelectionTool : Tool
    {
        public string ToolName { get { return "Selection"; } }
        public int ToolId { get { return 3; } }

        public Rectangle Execute(Point firstPoint, Point secondPoint)
        {
            return new Rectangle();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class EraserTool : Tool
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

        public EraserTool()
        {
            size = 1;
        }

        public void Execute(WriteableBitmap bmp, Point currentPoint)
        {
            bmp.MyDrawCircle((int)currentPoint.X, (int)currentPoint.Y, Size, Colors.Transparent);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
