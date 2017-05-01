using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.Common
{
    public static class DenseVectorExtention
    {
        public static double MNorm(this DenseVector x, DenseMatrix M)
        {
            var Mx = M * x;
            return Math.Sqrt(Math.Abs(x * Mx));
        }
    }
}
