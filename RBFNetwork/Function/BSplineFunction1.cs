using System;

namespace RBFNetwork.Function
{
    public class BSplineFunction1 : BasicFunction
    {
        public  BSplineFunction1(int dimensions)
            : base(dimensions, dimensions)
        {
        }

        public override double Calculate(double[] x)
        {
            var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
                res *= Calculate(x[dim], Center[dim].Value, Parameters[dim].Value);
            return res;
         }

        private double Calculate(double x, double C, double A)
        {
            var a = Math.Exp(A);
            var h = a / 4.0;

            var res = 0.0;
            if (x > C + a)
                return res;

            res += 1.0 / (6.0 * Math.Pow(h, 4)) * Math.Pow(C + a - x, 3);

            if (x > C + 3 * h)
                return res;
            res += -4.0 / (6.0 * Math.Pow(h, 4)) * Math.Pow(C + 3 * h - x, 3);

            if (x > C + 2 * h)
                return res;
            res += 6.0 / (6.0 * Math.Pow(h, 4)) * Math.Pow(C + 2 * h - x, 3);

            if (x > C + h)
                return res;
            res += -4.0 / (6.0 * Math.Pow(h, 4)) * Math.Pow(C + h - x, 3);

            if (x > C)
                return res;
            res += 1.0 / (6.0 * Math.Pow(h, 4)) * Math.Pow(C - x, 3);
            return res;
        }
        
        public override double FirstDerivation(double[] p, int cDim)
        {
            var res = 1.0;
            return res;
        }

        public override double SecondDerivation(double[] p, int cDim)
        {
            var res = 1.0;
            return res;
        }
    }
}
