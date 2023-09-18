using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PixelDrawer.Model
{
    static class WriteableBitmapExEx
    {
        public static void MyFillEllipseCentered(this WriteableBitmap bmp, int xc, int yc, int xr, int yr, Color color)
        {
            int color2 = MyConvertColor(color);
            bmp.MyFillEllipseCentered(xc, yc, xr, yr, color2);
        }

        public unsafe static void Draw(this WriteableBitmap bmp, int xc, int yc, int xr, int yr, Color color)
        {
            int intColor = MyConvertColor(color);
            using BitmapContext bitmapContext = bmp.GetBitmapContext();
            int* pixels = bitmapContext.Pixels;
            int width = bitmapContext.Width;
            int height = bitmapContext.Height;
            if (xr < 1 || yr < 1 || xc - xr >= width || xc + xr < 0 || yc - yr >= height || yc + yr < 0)
            {
                return;
            }
        }

        private unsafe static void MyFillEllipseCentered(this WriteableBitmap bmp, int xc, int yc, int xr, int yr, int color, bool doAlphaBlend = false)
        {
            using BitmapContext bitmapContext = bmp.GetBitmapContext();
            int* pixels = bitmapContext.Pixels;
            int width = bitmapContext.Width;
            int height = bitmapContext.Height;
            if (xr < 1 || yr < 1 || xc - xr >= width || xc + xr < 0 || yc - yr >= height || yc + yr < 0)
            {
                return;
            }

            int num = xr;
            int num2 = 0;
            int num3 = xr * xr << 1;
            int num4 = yr * yr << 1;
            int num5 = yr * yr * (1 - (xr << 1));
            int num6 = xr * xr / 4;
            int num7 = 0;
            int num8 = num4 * xr;
            int num9 = 0;
            int num10 = (color >> 24) & 0xFF;
            int sr = (color >> 16) & 0xFF;
            int sg = (color >> 8) & 0xFF;
            int sb = color & 0xFF;
            bool flag = !doAlphaBlend || num10 == 255;
            int num13;
            int num14;
            int num11;
            int num12;
            while (num8 >= num9)
            {
                num11 = yc + num2;
                num12 = yc - num2 - 1;
                if (num11 < 0)
                {
                    num11 = 0;
                }

                if (num11 >= height)
                {
                    num11 = height - 1;
                }

                if (num12 < 0)
                {
                    num12 = 0;
                }

                if (num12 >= height)
                {
                    num12 = height - 1;
                }

                num13 = num11 * width;
                num14 = num12 * width;
                int num15 = xc + num;
                int num16 = xc - num;
                if (num15 < 0)
                {
                    num15 = 0;
                }

                if (num15 >= width)
                {
                    num15 = width - 1;
                }

                if (num16 < 0)
                {
                    num16 = 0;
                }

                if (num16 >= width)
                {
                    num16 = width - 1;
                }

                if (flag)
                {
                    for (int i = num16; i <= num15; i++)
                    {
                        pixels[i + num13] = color;
                        pixels[i + num14] = color;
                    }
                }
                else
                {
                    for (int j = num16; j <= num15; j++)
                    {
                        pixels[j + num13] = MyAlphaBlendColors(pixels[j + num13], num10, sr, sg, sb);
                        pixels[j + num14] = MyAlphaBlendColors(pixels[j + num14], num10, sr, sg, sb);
                    }
                }

                num2++;
                num9 += num3;
                num7 += num6;
                num6 += num3;
                if (num5 + (num7 << 1) > 0)
                {
                    num--;
                    num8 -= num4;
                    num7 += num5;
                    num5 += num4;
                }
            }

            num = 0;
            num2 = yr;
            num11 = yc + num2;
            num12 = yc - num2;
            if (num11 < 0)
            {
                num11 = 0;
            }

            if (num11 >= height)
            {
                num11 = height - 1;
            }

            if (num12 < 0)
            {
                num12 = 0;
            }

            if (num12 >= height)
            {
                num12 = height - 1;
            }

            num13 = num11 * width;
            num14 = num12 * width;
            num5 = yr * yr;
            num6 = xr * xr * (1 - (yr << 1));
            num7 = 0;
            num8 = 0;
            num9 = num3 * yr;
            while (num8 <= num9)
            {
                int num15 = xc + num;
                int num16 = xc - num;
                if (num15 < 0)
                {
                    num15 = 0;
                }

                if (num15 >= width)
                {
                    num15 = width - 1;
                }

                if (num16 < 0)
                {
                    num16 = 0;
                }

                if (num16 >= width)
                {
                    num16 = width - 1;
                }

                if (flag)
                {
                    for (int k = num16; k <= num15; k++)
                    {
                        pixels[k + num13] = color;
                        pixels[k + num14] = color;
                    }
                }
                else
                {
                    for (int l = num16; l <= num15; l++)
                    {
                        pixels[l + num13] = MyAlphaBlendColors(pixels[l + num13], num10, sr, sg, sb);
                        pixels[l + num14] = MyAlphaBlendColors(pixels[l + num14], num10, sr, sg, sb);
                    }
                }

                num++;
                num8 += num4;
                num7 += num5;
                num5 += num4;
                if (num6 + (num7 << 1) > 0)
                {
                    num2--;
                    num11 = yc + num2;
                    num12 = yc - num2;
                    if (num11 < 0)
                    {
                        num11 = 0;
                    }

                    if (num11 >= height)
                    {
                        num11 = height - 1;
                    }

                    if (num12 < 0)
                    {
                        num12 = 0;
                    }

                    if (num12 >= height)
                    {
                        num12 = height - 1;
                    }

                    num13 = num11 * width;
                    num14 = num12 * width;
                    num9 -= num3;
                    num7 += num6;
                    num6 += num3;
                }
            }
        }

        private static int MyAlphaBlendColors(int pixel, int sa, int sr, int sg, int sb)
        {
            int num = (pixel >> 24) & 0xFF;
            int num2 = (pixel >> 16) & 0xFF;
            int num3 = (pixel >> 8) & 0xFF;
            int num4 = pixel & 0xFF;
            return (sa + (num * (255 - sa) * 32897 >> 23) << 24) | (sr + (num2 * (255 - sa) * 32897 >> 23) << 16) | (sg + (num3 * (255 - sa) * 32897 >> 23) << 8) | (sb + (num4 * (255 - sa) * 32897 >> 23));
        }
        
        public static int MyConvertColor(Color color)
        {
            int result = 0;
            if (color.A != 0)
            {
                int num = color.A + 1;
                result = (color.A << 24) | ((byte)(color.R * num >> 8) << 16) | ((byte)(color.G * num >> 8) << 8) | (byte)(color.B * num >> 8);
            }

            return result;
        }

        public static unsafe void MyDrawCircle(this WriteableBitmap bmp, int centerX, int centerY, int size, Color color)
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
                //drawpixel(centerX + x, centerY + y);
                //drawpixel(centerX + x, centerY - y);
                //drawpixel(centerX - x, centerY + y);
                //drawpixel(centerX - x, centerY - y);
                //drawpixel(centerX + y, centerY + x);
                //drawpixel(centerX + y, centerY - x);
                //drawpixel(centerX - y, centerY + x);
                //drawpixel(centerX - y, centerY - x);
                //добавить проверки границ
                pixels[(centerY + y) * width + centerX - x] = MyAlphaBlendColors(pixels[(centerY + y) * width + centerX - x], sa, sr, sg, sb);
                pixels[(centerY - y) * width + centerX - x] = MyAlphaBlendColors(pixels[(centerY - y) * width + centerX - x], sa, sr, sg, sb);
                pixels[(centerY + x) * width + centerX - y] = MyAlphaBlendColors(pixels[(centerY + x) * width + centerX - y], sa, sr, sg, sb);
                pixels[(centerY - x) * width + centerX - y] = MyAlphaBlendColors(pixels[(centerY - x) * width + centerX - y], sa, sr, sg, sb);

                delta += delta < 0 ? 4 * x + 6 : 4 * (x - y--) + 10;
                ++x;
            }
        }
    }
}
