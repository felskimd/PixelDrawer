using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;

namespace PixelDrawer.Model
{
    static class DrawingMethods
    {
        private static int MyAlphaBlendColors(int pixel, int sa, int sr, int sg, int sb)
        {
            int num = (pixel >> 24) & 0xFF;
            int num2 = (pixel >> 16) & 0xFF;
            int num3 = (pixel >> 8) & 0xFF;
            int num4 = pixel & 0xFF;
            return (sa + (num * (255 - sa) * 32897 >> 23) << 24) | (sr + (num2 * (255 - sa) * 32897 >> 23) << 16) | (sg + (num3 * (255 - sa) * 32897 >> 23) << 8) | (sb + (num4 * (255 - sa) * 32897 >> 23));
        }
        
        public static int MyConvertColor(System.Windows.Media.Color color)
        {
            int result = 0;
            if (color.A != 0)
            {
                int num = color.A + 1;
                result = (color.A << 24) | ((byte)(color.R * num >> 8) << 16) | ((byte)(color.G * num >> 8) << 8) | (byte)(color.B * num >> 8);
            }

            return result;
        }

        public static unsafe void MyDrawCircle(this WriteableBitmap bmp, int centerX, int centerY, int size, System.Windows.Media.Color color)
        {
            using BitmapContext bitmapContext = bmp.GetBitmapContext();
            int* pixels = bitmapContext.Pixels;
            int width = bitmapContext.Width;
            int height = bitmapContext.Height;
            int convColor = MyConvertColor(color);
            int sa = (convColor >> 24) & 0xFF;
            int sr = (convColor >> 16) & 0xFF;
            int sg = (convColor >> 8) & 0xFF;
            int sb = convColor & 0xFF;
            var x = 0;
            var y = size;
            var delta = 3 - 2 * y;
            while (x <= y)
            {
                int i1 = centerX - x;
                while (i1 <= centerX + x) 
                {
                    if (i1 >= 0 && i1 < width && centerY + y < height && centerY + y >= 0) 
                        pixels[(centerY + y) * width + i1] = MyAlphaBlendColors(pixels[(centerY + y) * width + i1], sa, sr, sg, sb);
                    if (i1 >= 0 && i1 < width && centerY - y < height && centerY - y >= 0)
                        pixels[(centerY - y) * width + i1] = MyAlphaBlendColors(pixels[(centerY - y) * width + i1], sa, sr, sg, sb);
                    i1++;
                }
                int i2 = centerX - y;
                while (i2 <= centerX + y) 
                {
                    if (i2 >= 0 && i2 < width && centerY + x < height && centerY + x >= 0)
                        pixels[(centerY + x) * width + i2] = MyAlphaBlendColors(pixels[(centerY + x) * width + i2], sa, sr, sg, sb);
                    if (i2 >= 0 && i2 < width && centerY - x < height && centerY - x >= 0)
                        pixels[(centerY - x) * width + i2] = MyAlphaBlendColors(pixels[(centerY - x) * width + i2], sa, sr, sg, sb);
                    i2++;
                }
                delta += delta < 0 ? 4 * x + 6 : 4 * (x - y--) + 10;
                ++x;
            }
        }

        public static unsafe void MyDrawLine(this WriteableBitmap bmp, Point startPosition, Point endPosition, int thickness, System.Windows.Media.Color color)
        {
            //using BitmapContext bitmapContext = bmp.GetBitmapContext();
            //int* pixels = bitmapContext.Pixels;
            //int width = bitmapContext.Width;
            //int height = bitmapContext.Height;
            //int convColor = MyConvertColor(color);
            //int sa = (convColor >> 24) & 0xFF;
            //int sr = (convColor >> 16) & 0xFF;
            //int sg = (convColor >> 8) & 0xFF;
            //int sb = convColor & 0xFF;
            int x0 = startPosition.X;
            int y0 = startPosition.Y;
            int x1 = endPosition.X;
            int y1 = endPosition.Y;

            bool steep = Math.Abs(endPosition.Y - startPosition.Y) > Math.Abs(endPosition.X - startPosition.X); // Проверяем рост отрезка по оси икс и по оси игрек
            // Отражаем линию по диагонали, если угол наклона слишком большой
            if (steep)
            {
                int temp = x0;
                x0 = y0;
                y0 = temp;
                temp = x1;
                x1 = y1;
                y1 = temp;
                //Swap(ref x0, ref y0); // Перетасовка координат вынесена в отдельную функцию для красоты
                //Swap(ref x1, ref y1);
            }
            // Если линия растёт не слева направо, то меняем начало и конец отрезка местами
            if (x0 > x1)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;
                temp = y0;
                y0 = y1;
                y1 = temp;
                //Swap(ref x0, ref x1);
                //Swap(ref y0, ref y1);
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2; // Здесь используется оптимизация с умножением на dx, чтобы избавиться от лишних дробей
            int ystep = (y0 < y1) ? 1 : -1; // Выбираем направление роста координаты y
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                //DrawPoint(steep ? y : x, steep ? x : y); // Не забываем вернуть координаты на место
                MyDrawCircle(bmp, steep ? y : x, steep ? x : y, thickness, color);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }
    }
}