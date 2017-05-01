using System;

namespace testtr
{
    public class OFunc
    {
        public double evaluateFunction(double[] x)
        {
            double term1 = 100 * Math.Pow((x[1] - Math.Pow(x[0], 2.0)), 2.0);
            double term2 = Math.Pow((1 - x[0]), 2.0);
            return term1 + term2;
        }
    }
}
