using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PixelDrawer.Model;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows;
using PixelDrawer.ViewModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace PixelDrawer
{
    static class FileSaveLoad
    {
        public static void SaveFileAs(string fileName, Project project, Canvas canvas)
        {
            switch (fileName.Split('.').Last())
            {
                case "png":
                    var pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(PackLayers(canvas)));
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                        pngEncoder.Save(fileStream);
                    break;
                case "jpeg":
                    var jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.Frames.Add(BitmapFrame.Create(PackLayers(canvas)));
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                        jpegEncoder.Save(fileStream);
                    break;
                case "bmp":
                    var bmpEncoder = new BmpBitmapEncoder();
                    bmpEncoder.Frames.Add(BitmapFrame.Create(PackLayers(canvas)));
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                        bmpEncoder.Save(fileStream);
                    break;
                case "gif":
                    var gifEncoder = new GifBitmapEncoder();
                    foreach (var layer in project.Layers)
                    {
                        if (layer.IsVisible)
                            gifEncoder.Frames.Add(BitmapFrame.Create(layer.Bitmap));
                    }
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                        gifEncoder.Save(fileStream);
                    break;
            }
        }

        private static RenderTargetBitmap PackLayers(Canvas canvas)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            var renderTarget = new RenderTargetBitmap(
                (int)bounds.Width,
                (int)bounds.Height,
                96,
                96,
                PixelFormats.Pbgra32);

            var visual = new DrawingVisual();

            using (var context = visual.RenderOpen())
            {
                var visualBrush = new VisualBrush(canvas);
                context.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
            }

            renderTarget.Render(visual);
            return renderTarget;
        }

        public static void SaveProject(Project project) 
        {
            using (var stream = File.Open(System.Environment.CurrentDirectory + "\\" + project.Name + ".pdpr", FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(project.Name);
                    writer.Write(project.Width); 
                    writer.Write(project.Height);
                    writer.Write(project.Layers.Count);
                    foreach (var layer in project.Layers)
                    {
                        writer.Write(layer.Name);
                        writer.Write(layer.IsVisible);
                        writer.Write(layer.Bitmap.ToByteArray());
                    }
                }
            }
                
        }

        public static Project OpenProject(string path)
        {
            string projectName;
            int width;
            int height;
            var layers = new ObservableCollection<Layer>();
            int layersCount;
            using (var stream = File.Open(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    projectName = reader.ReadString();
                    width = reader.ReadInt32();
                    height = reader.ReadInt32();
                    layersCount = reader.ReadInt32();
                    for (int i = 0; i < layersCount; i++)
                    {
                        var layerName = reader.ReadString();
                        bool isVisible = reader.ReadBoolean();
                        var bmp = BitmapFactory.New(width, height);
                        var byteArray = reader.ReadBytes(width * height * 4);
                        bmp.FromByteArray(byteArray);
                        layers.Add(new Layer(layerName, bmp, isVisible));
                    }
                }
            }
            var newProject = new Project(projectName, layers, layersCount, width, height);
            return newProject;
        }
    }
}
