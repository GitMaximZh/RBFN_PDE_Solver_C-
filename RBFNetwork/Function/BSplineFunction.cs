using System;

namespace RBFNetwork.Function
{
    public class BSplineFunction : BasicFunction
    {
        public  BSplineFunction(int dimensions)
            : base(dimensions, dimensions * 5)
        {
        }

        public override double Calculate(double[] x)
        {
            var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
                res *= SecondDerivation(x, 0); 
                    //Calculate(x[dim], Center[dim].Value, Parameters[dim*5].Value, Parameters[dim*5 + 1].Value,
                      //           Parameters[dim*5 + 2].Value, Parameters[dim*5 + 3].Value, Parameters[dim*5 + 4].Value);
            return res;
         }

        private double Calculate(double x, double C, double A, double Phi, double Lambda, double c1, double c2)
        {
            var g = Scale(Lambda);
            var f = Scale(Phi);
            var a = Math.Exp(A);

            var res = c1 * x + c2;
            if (x > C + a)
                return res;

            res += 1.0 / 12.0 * Math.Pow(C + a - x, 4) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C + a * f - x, 4) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f * g)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C + a * f * g - x, 4) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C - x, 4) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);
            return res;
        }

        private double Scale(double x)
        {
            return 1.0 / Math.PI * Math.Atan(x) + 0.5; // Math.Exp(x) / (1.0 + Math.Exp(x));
        }

        public override double FirstDerivation(double[] p, int cDim)
        {
            var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
            {
                if (dim == cDim)
                    res *= CalculateFirstDerivation(p[dim], Center[dim].Value, Parameters[dim * 5].Value,
                                                    Parameters[dim * 5 + 1].Value,
                                                    Parameters[dim * 5 + 2].Value, Parameters[dim * 5 + 3].Value);
                else
                    res *= Calculate(p[dim], Center[dim].Value, Parameters[dim * 5].Value, Parameters[dim * 5 + 1].Value,
                                 Parameters[dim * 5 + 2].Value, Parameters[dim * 5 + 3].Value, Parameters[dim * 5 + 4].Value);
            }
            return res;
        }

        private double CalculateFirstDerivation(double x, double C, double A, double Phi, double Lambda, double c1)
        {
            var g = Scale(Lambda);
            var f = Scale(Phi);
            var a = Math.Exp(A);

            var res = c1;
            if (x > C + a)
                return res;

            res += -1.0 / 3.0 * Math.Pow(C + a - x, 3) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C + a * f - x, 3) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f * g)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C + a * f * g - x, 3) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C - x, 3) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);

            return res;
        }

        private double CalculateSecondDerivation(double x, double C, double A, double Phi, double Lambda)
        {
            var g = Scale(Lambda);
            var f = Scale(Phi);
            var a = Math.Exp(A);

            var res = 0.0;
            if (x > C + a)
                return res;

            res += Math.Pow(C + a - x, 2) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += Math.Pow(C + a * f - x, 2) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f * g)
                return res;
            res += Math.Pow(C + a * f * g - x, 2) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += Math.Pow(C - x, 2) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);
            return res;
        }

        public override double SecondDerivation(double[] p, int cDim)
        {
            var res = 1.0;
            for (int dim = 0; dim < Dimension; dim++)
            {
                if (dim == cDim)
                    res *= CalculateSecondDerivation(p[dim], Center[dim].Value, Parameters[dim * 5].Value,
                                                    Parameters[dim * 5 + 1].Value,
                                                    Parameters[dim * 5 + 2].Value);
                else
                    res *= Calculate(p[dim], Center[dim].Value, Parameters[dim * 5].Value, Parameters[dim * 5 + 1].Value,
                                 Parameters[dim * 5 + 2].Value, Parameters[dim * 5 + 3].Value, Parameters[dim * 5 + 4].Value);
            }
            return res;
        }
    }
}
