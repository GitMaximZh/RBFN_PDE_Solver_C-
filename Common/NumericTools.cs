using System;
using Common;

namespace Common
{
    public static class NumericTools
    {
        public const double h = 0.0001;
        public const double H = 0.0002;
        public const double h2 = 0.00000001;
        public const double H2 = 0.00000004;

        public static double FirstOrderDerivation(Func<Point, double> func, Point point, int dimension)
        {
            var left = (Point)point.Clone();
            left[dimension] += h;

            var dfh = (func(left) - func(point)) / h;

            //left[dimension] += h;
            //var df2h = (func(left) - func(point)) / H;

            return dfh;// +(dfh - df2h) / 3.0;

            //var left = (Point)point.Clone();
            //var right = (Point)point.Clone();
            //left[dimension] += h;
            //right[dimension] -= h;

            //var left2 = (Point)point.Clone();
            //var right2 = (Point)point.Clone();

            //left2[dimension] += 2 * h;
            //right2[dimension] -= 2 * h;

            //var d =  (func(left) - func(right)) / (2*h);
            //var d2 = (func(left2) - func(right2)) / (2 * 2 * h);

            //return d + (d - d2) / 3.0;
        }

        public static double SecondOrderDerivation(Func<Point, double> func, Point point, int dimension)
        {
            //var left = (Point)point.Clone();
            //var right = (Point)point.Clone();
            
            //left[dimension] += h;
            //right[dimension] -= h;

            //var ddfh = (func(left) - 2 * func(point) + func(right)) / h2;

            //left[dimension] += h;
            //right[dimension] -= h;

            //var ddf2h = (func(left) - 2 * func(point) + func(right)) / H2;
            //return ddfh;// +(ddfh - ddf2h) / 3.0;

            var left = (Point)point.Clone();
            var right = (Point)point.Clone();

            left[dimension] += 2*h;
            right[dimension] -= 2*h;

            var left2 = (Point)point.Clone();
            var right2 = (Point)point.Clone();

            left2[dimension] += 4 * h;
            right2[dimension] -= 4 * h;

            var d = (func(left) - 2 * func(point) + func(right)) / (4 * h2);
            var d2 = (func(left2) - 2 * func(point) + func(right2)) / (4 * 4 * h2);

            return d + (d - d2) / 3.0;
        }
    }
}
