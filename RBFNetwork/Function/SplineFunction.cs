using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

namespace RBFNetwork.Function
{
    public class SplineFunction : BasicFunction
    {
        private readonly int n;
        private IList<double> paramerersKeeper = new List<double>();

        private DenseVector param;
        private IList<double> h;

        public SplineFunction(int dimensions, int pointsCount)
            : base(dimensions, (1 + 2 * pointsCount) * dimensions)
        {
            n = pointsCount + 1;
        }

        public override double Calculate(double[] x)
        {
            var current = Parameters.Select(p => p.Value).ToList();
            if (!paramerersKeeper.SequenceEqual(current))
            {
                BuildSpline();
                paramerersKeeper = current;
            }
            var c = Center[0].Value;
            var a = Math.Exp(Parameters[0].Value);// *Parameters[0].Value;

            if (x[0] <= c || x[0] >= c + a)
                return 0;
            var x_ = 0.0;
            for (int i = 0; i < n; i++)
            {
                x_ += h[i];
                if (x[0] < c + x_)
                    return param[4*i] + param[4*i + 1]*(x[0] - x_ - c) +
                           param[4 * i + 2] * Math.Pow((x[0] - x_ - c), 2) +
                           param[4*i + 3]*Math.Pow((x[0] - x_ - c), 3);
            }
            return 0;
        }

        private double Scale(double x)
        {
            return Math.Exp(x) / (1.0 + Math.Exp(x)); // 1.0 / Math.PI * Math.Atan(x) + 0.5; //
        }

        private void BuildSpline()
        {
            var a = Math.Exp(Parameters[0].Value);// *Parameters[0].Value;

            h = new List<double>(Enumerable.Repeat(0.0, n));
            var prev = a;
            //var step = a/n;
            for (int i = n - 2; i >= 0; i--)
            {
                var cur = Scale(Parameters[n + i].Value) * prev;
                h[i + 1] = prev - cur; //step;
                prev = cur;
            }
            h[0] = prev; //step;

            DenseMatrix A = new DenseMatrix(4 * n, 4 * n);
            DenseVector b = new DenseVector(4 * n);

            int row = 0;
            A[row, 0] = 1; A[row, 1] = -h[0]; A[row, 2] = Math.Pow(h[0], 2); A[row, 3] = -Math.Pow(h[0], 3); b[row] = 0; row++;
            for (int i = 0; i < n - 1; i++)
            {
                A[row, i * 4] = 1; b[row] = Math.Pow(Parameters[i + 1].Value, 3); row++;
                A[row, i * 4] = 1; A[row, (i + 1) * 4] = -1; A[row, (i + 1) * 4 + 1] = h[i + 1]; A[row, (i + 1) * 4 + 2] = -Math.Pow(h[i + 1], 2); A[row, (i + 1) * 4 + 3] = Math.Pow(h[i + 1], 3); b[row] = 0; row++;
                A[row, i * 4 + 1] = 1; A[row, (i + 1) * 4 + 1] = -1; A[row, (i + 1) * 4 + 2] = 2 * h[i + 1]; A[row, (i + 1) * 4 + 3] = -3 * Math.Pow(h[i + 1], 2); b[row] = 0; row++;
                A[row, i * 4 + 2] = 1; A[row, (i + 1) * 4 + 2] = -1; A[row, (i + 1) * 4 + 3] = 3 * h[i + 1]; b[row] = 0; row++;
            }
            A[row, (n - 1) * 4] = 1; b[row] = 0; row++;
            A[row, 1] = 1; A[row, 2] = -2 * h[0]; A[row, 3] = 3 * Math.Pow(h[0], 2); b[row] = 0; row++;
            A[row, (n - 1) * 4 + 1] = 1; b[row] = 0;

            param = new DenseVector(A.Inverse() * b);
        }

        public override double FirstDerivation(double[] x, int dim)
        {
            return 0.0;
        }

        public override double SecondDerivation(double[] x, int dim)
        {
            return 0.0;
        }
    }
}
