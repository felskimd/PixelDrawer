using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PixelDrawer.Model
{
    internal static class DrawingInterpolation
    {
        public static System.Windows.Point CatmullRomInterpolation2d(System.Windows.Point value1, System.Windows.Point value2, System.Windows.Point value3, System.Windows.Point value4, double amount)
        {
            return new System.Windows.Point(
                (int)CatmullRomInterpolation1d(value1.X, value2.X, value3.X, value4.X, amount),
                (int)CatmullRomInterpolation1d(value1.Y, value2.Y, value3.Y, value4.Y, amount));
        }

        public static double CatmullRomInterpolation1d(double value1, double value2, double value3, double value4, double amount)
        {
            // Internally using doubles not to lose precission
            double amountSquared = amount * amount;
            double amountCubed = amountSquared * amount;
            return (double)(0.5 * (2.0 * value2 +
                (value3 - value1) * amount +
                (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }
    }
}
