using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testFunctions
{
    public class MQ
    {
        private double Center;
        private double Width;
        public MQ(double c, double a)
        {
            Center = c;
            Width = a;
        }

        public double Calculate(double x)
        {
            double value = 0;
            value += Math.Pow(x - Center, 2);
            return Math.Sqrt(Width * Width + value);
        }

        public double FirstDerivation(double x)
        {
            return (x - Center) / Calculate(x);
        }

        public double SecondDerivation(double x)
        {
            var value = Calculate(x);
            return -Math.Pow(x - Center, 2) / Math.Pow(value, 3) + 1.0 / value;
        }
    }
}
