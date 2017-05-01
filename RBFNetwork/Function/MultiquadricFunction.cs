using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RBFNetwork.Function
{
    public class MultiquadricFunction : BasicFunction
    {
        public MultiquadricFunction(int dimensions)
            : base(dimensions, 1)
        {

        }

        public override double Calculate(double[] x)
        {
            double value = 1;
            for (var i = 0; i < Center.Length; i++)
            {
                value *= Math.Sqrt(Parameters[0] * Parameters[0] + Math.Pow(x[i] - Center[i], 2));
            }
            return value;
        }

        public override double FirstDerivation(double[] x, int cDim)
        {
            var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
            {
                if (dim == cDim)
                    res *= (x[dim] - Center[dim]) / Math.Sqrt(Parameters[0] * Parameters[0] + Math.Pow(x[dim] - Center[dim], 2));
                else
                    res *= Math.Sqrt(Parameters[0] * Parameters[0] + Math.Pow(x[dim] - Center[dim], 2));
            }
            return res;
        }

        public override double SecondDerivation(double[] x, int cDim)
        {
           var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
            {
                if (dim == cDim)
                {
                    var value = Math.Sqrt(Parameters[0]*Parameters[0] + Math.Pow(x[dim] - Center[dim], 2));
                    res *= - Math.Pow(x[dim] - Center[dim], 2) / Math.Pow(value, 3) + 1.0 / value;
                }
                else
                    res *= Math.Sqrt(Parameters[0] * Parameters[0] + Math.Pow(x[dim] - Center[dim], 2));
            }

            return res;
        }
    }
}
