using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace testFunctions
{
    public class Spline
    {
        private DenseVector p;
        private double c;
        private double a;
        private int n;
        private double h;
        
        public  Spline(double c, double a, IList<double> fs)
        {
            this.c = c;
            this.a = a;

            n = fs.Count + 1;
            h = a / n;
            double h2 = Math.Pow(h, 2), h3 = Math.Pow(h, 3); 

            DenseMatrix A = new DenseMatrix(4 * n, 4 * n);
            DenseVector b = new DenseVector(4 * n);

            int row = 0;
            A[row, 0] = 1; A[row, 1] = -h; A[row, 2] = h2; A[row, 3] = -h3; b[row] = 0; row++;
            for (int i = 0; i < n - 1; i++)
            {
                A[row, i * 4] = 1; b[row] = fs[i]; row++;
                A[row, i * 4] = 1; A[row, (i + 1) * 4] = -1; A[row, (i + 1) * 4 + 1] = h; A[row, (i + 1) * 4 + 2] = -h2; A[row, (i + 1) * 4 + 3] = h3; b[row] = 0; row++;
                A[row, i * 4 + 1] = 1; A[row, (i + 1) * 4 + 1] = -1; A[row, (i + 1) * 4 + 2] = 2 * h; A[row, (i + 1) * 4 + 3] = -3 * h2; b[row] = 0; row++;
                A[row, i * 4 + 2] = 1; A[row, (i + 1) * 4 + 2] = -1; A[row, (i + 1) * 4 + 3] = 3 * h; b[row] = 0; row++;
            }
            A[row, (n - 1) * 4] = 1; b[row] = 0; row++;
            A[row, 1] = 1; A[row, 2] = -2 * h; A[row, 3] = 3 * h2; b[row] = 0; row++;
            A[row, (n - 1) * 4 + 1] = 1; b[row] = 0;

            p = new DenseVector(A.Inverse() * b);
        }

        public double Value(double x)
        {
           if (x <= c || x >= c + a)
                return 0;
           for (int i = 0; i < n; i++)
               if(x < c + h * (i + 1))
                   return p[4 * i] + p[4 * i + 1] * (x - h * (i + 1)) + p[4 * i + 2] * Math.Pow((x - h * (i + 1)), 2) + p[4 * i + 3] * Math.Pow((x - h * (i + 1)), 3);
            return 0;
        }
    }
}
