using System;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.TrustRegion
{
    public class PCGResolver : ISubProblemResolver
    {
        public double Ksi { get; set; }

        public PCGResolver()
        {
            Ksi = 0.0001;
        }

        public DenseVector Resolve(SubProblem problem)
        {
            var gradient = problem.Gradient;
            var hessian = problem.Hessian;
            var tr = problem.TrustRegion;

            var rk = -gradient;
            var H = hessian;
            var sk = new DenseVector(gradient.Count, 0);
            var pk = rk;
            

            var M = new DenseMatrix(gradient.Count, gradient.Count, 0);
            var M_1 = new DenseMatrix(gradient.Count, gradient.Count, 0);

            var z = new DenseVector(gradient.Count, 0);
            for (int i = 0; i < z.Count; i++)
            {
                if (H[i, i] == 0.0)
                {
                    M[i, i] = 1.0;
                    M_1[i, i] = 1.0;
                }
                else
                {
                    M[i, i] = Math.Sqrt(H[i, i]);
                    M_1[i, i] = 1 / M[i, i];
                }
                
            }
            z = M_1 * rk;
            var grLength = gradient.MNorm(M);
            //var tr = Norm(new DenseVector(Gradient.Count, 0), M);

            pk = z;

            while (true)
            {
                var k = pk * (H * pk);

                if (k <= 0)
                    return GradientStep(sk, pk, M, tr);

                var a = rk * z / k;
                if ((sk + a * pk).MNorm(M) > tr)
                    return GradientStep(sk, pk, M, tr);

                sk = sk + a * pk;
                
                var rk1 = rk - a * H * pk;
                if (rk1.MNorm(M) / grLength <= Ksi)
                    return sk;

                var zk1 = new DenseVector(gradient.Count, 0);
                zk1 = M_1 * rk1;
                

                var b = (rk1 * zk1) / (rk * z);
                pk = zk1 + b * pk;

                rk = rk1;
                z = zk1;
            }
        }

        private DenseVector GradientStep(DenseVector sk, DenseVector pk, DenseMatrix M, double tr)
        {
            var l = (-(sk * (M * pk)) + Math.Sqrt(Math.Pow(sk * (M * pk), 2) + pk * (M * pk) * (tr * tr - sk * (M * sk)))) / (pk * (M * pk));
            return sk + l * pk;
        }
    }
}
