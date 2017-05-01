using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBFNetwork.Function
{
    public class AsymmetricGaussianFunction2d : BasicFunction
    {
        public AsymmetricGaussianFunction2d(int dimensions)
            : base(dimensions, dimensions)
        {

        }

        public override double Calculate(double[] x)
        {
            double value = 0;
            for (var i = 0; i < Center.Length; i++)
            {
                value += Math.Pow(x[i] - Center[i], 2) / (2.0 * Parameters[i] * Parameters[i]);
            }
            return Math.Exp(-value);

            //double value = 0;
            //var cos0 = 1.0;// Math.Cos(Widths[2]);
            //var sin0 = 0.0;// Math.Sin(Widths[2]);
            //var sin20 = 0;// Math.Sin(2 * Widths[2]);

            //var a = cos0 * cos0 / (2 * Math.Pow(Widths[0], 2)) + sin0 * sin0 / (2 * Math.Pow(Widths[1], 2));
            //var c = cos0 * cos0 / (2 * Math.Pow(Widths[1], 2)) + sin0 * sin0 / (2 * Math.Pow(Widths[0], 2));
            //var b = -sin20 / (4 * Math.Pow(Widths[0], 2)) + sin20 / (4 * Math.Pow(Widths[1], 2));

            //value += a * Math.Pow(x[0] - Centers[0], 2);
            //value += c * Math.Pow(x[1] - Centers[1], 2);
            //value += 2 * b * (x[0] - Centers[0]) * (x[1] - Centers[1]);

            //return Math.Exp(-value);
        }

        public override double FirstDerivation(double[] x, int dim)
        {

            return -1.0 / (Parameters[dim] * Parameters[dim]) * Value(x) * (x[dim] - Center[dim]);
        }

        public override double SecondDerivation(double[] x, int dim)
        {
            return Value(x) * (Math.Pow((x[dim] - Center[dim]), 2) - Parameters[dim != 2 ? 0 : 1] * Parameters[dim]) / Math.Pow(Parameters[dim], 4);
        }
    }
}
