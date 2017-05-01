using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.TrustRegion
{
    public class TrustRegionMethod : OptimizationMethod
    {
        private static readonly Random Rand = new Random(100);

        public double Mu1 { get; set; }
        public double Mu2 { get; set; }

        public double Gamma1 { get; set; }
        public double Gamma2 { get; set; }

        public double TrustRegion { get; set; }
        public double MinTrustRegion { get; set; }

        public new ITwiceDifferentiableFunction Function { get; private set; }

        private ISubProblemResolver Resolver { get; set; }

        public TrustRegionMethod(ITwiceDifferentiableFunction function, Point startPoint, ISubProblemResolver resolver)
            : base(function, startPoint)
        {
            Function = function;
            Resolver = resolver;

            Mu1 = 0.2;
            Mu2 = 0.7;

            Gamma1 = 0.5;
            Gamma2 = 0.7;

            MinTrustRegion = 0.000000001;
            TrustRegion = 1.0;
        }

        public override bool DoOptimizationStep()
        {
            var xk = CurrentPoint;
            var tr = TrustRegion;

            DenseMatrix hessian = Function.Hessian(xk);
            DenseVector gradient = Function.Gradient(xk);
            //Console.WriteLine("Cond: {0}", hessian.ConditionNumber());
            var subProblem = new SubProblem {Gradient = gradient, Hessian = hessian};

            while (!breakStep)
            {
                if (tr < MinTrustRegion)
                    break;

                subProblem.TrustRegion = tr;

                var s = Resolver.Resolve(subProblem);
                var xk1 = xk.Plus(s);

                var fk = Function.Value(xk);
                var fk1 = Function.Value(xk1);

                var mk = fk;
                var mk1 = AproximateValue(mk, gradient, hessian, s);

                var p = (fk - fk1)/(mk - mk1);
                tr = CorrectTrustRegion(p, tr);

                if (p > Mu1)
                {
                    CurrentPoint = xk1;
                    CurrentValue = fk1;

                    TrustRegion = tr;

                    return true;
                }
            }
            return false;
        }


        private double CorrectTrustRegion(double p, double tr)
        {
            if (p >= Mu2)
                return tr + (tr / Gamma2 - tr) * Rand.NextDouble();
            if (p >= Mu1 && p < Mu2)
                return Gamma2 * tr + (tr - Gamma2 * tr) * Rand.NextDouble();
            return Gamma1 * tr + (Gamma2 * tr - Gamma1 * tr) * Rand.NextDouble();
        }

        private double AproximateValue(double mk, DenseVector gradient, DenseMatrix hessian, DenseVector s)
        {
            return mk + gradient * s + 0.5 * s * (hessian * s);
        }
    }
}
