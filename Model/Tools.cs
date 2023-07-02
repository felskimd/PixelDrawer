﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

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

        public void Execute(WriteableBitmap bmp, System.Windows.Point currentPoint, System.Windows.Media.Color color)
        {
            bmp.MyFillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, color);
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

    public class SelectionTool : Tool
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

        public void Execute(WriteableBitmap bmp, System.Windows.Point currentPoint)
        {
            bmp.MyFillEllipseCentered((int)currentPoint.X, (int)currentPoint.Y, Size, Size, Colors.Transparent);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
