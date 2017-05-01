using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.TrustRegion
{
    public class CGResolver : ISubProblemResolver
    {
        public double Ksi { get; set; }

        public CGResolver()
        {
            Ksi = 0.0001; 
        }

        public DenseVector Resolve(SubProblem problem)
        {
            var gradient = problem.Gradient;
            var hessian = problem.Hessian;
            var tr = problem.TrustRegion;

            var grLength = gradient.Norm(2);

            var rk = -gradient;
            var H = hessian;
            var sk = new DenseVector(gradient.Count, 0);
            var pk = rk;

            while (true)
            {
                var k = pk * (H * pk);

                if (k <= 0)
                    return GradientStep(sk, pk, tr);

                var a = rk * rk / k;
                if ((sk + a * pk).Norm(2) > tr)
                    return GradientStep(sk, pk, tr);

                sk = sk + a * pk;
                
                var rk1 = rk - a * H * pk;
                if (rk1.Norm(2) / grLength <= Ksi)
                    return sk;

                var b = (rk1 * rk1) / (rk * rk);
                pk = rk1 + b * pk;

                rk = rk1;
            }
        }

        private DenseVector GradientStep(DenseVector sk, DenseVector pk, double tr)
        {
            var l = (-sk * pk + Math.Sqrt(Math.Pow(sk * pk, 2) + pk * pk * (tr * tr - sk * sk))) / (pk * pk);
            return sk + l * pk;
        }
    }
}
