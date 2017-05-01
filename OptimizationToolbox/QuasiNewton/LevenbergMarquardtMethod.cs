using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.QuasiNewton
{
    public class LevenbergMarquardtMethod : OptimizationMethod
    {
        public double Tau { get; set; }
        public double MinStep { get; set; }

        private double upsilon = 2;
        private double? mu;

        public new IJacobianFunction Function { get; private set; }

        public LevenbergMarquardtMethod(IJacobianFunction function, Point startPoint)
            : base(function, startPoint)
        {
            Tau = 0.1;
            MinStep = 0.00001;

            Function = function;
        }

        public override bool DoOptimizationStep()
        {
            while (!breakStep)
            {
                var jacobian = Function.Jacobian(CurrentPoint);
                if (!mu.HasValue)
                    mu = Tau*jacobian.Diagonal().Maximum();

                var difference = Function.Difference(CurrentPoint);

                var gradient = new DenseVector((jacobian.Transpose() *
                                                new DenseMatrix(difference.Count, 1, difference.ToArray())).Column(0));
                var A = jacobian.Transpose() * jacobian;


                var step = new DenseVector((A + mu.Value * DenseMatrix.Identity(A.ColumnCount)).Inverse() * (-gradient));
                if (step.Norm(2) < MinStep)
                    return false;

                var xk1 = CurrentPoint.Plus(step);

                var fk = Function.Value(CurrentPoint);
                var fk1 = Function.Value(xk1);
                var l0_lStep = 0.5 * step * (mu.Value * step - gradient);

                var p = (fk - fk1) / l0_lStep;
                if (p > 0)
                {
                    CurrentPoint = xk1;
                    CurrentValue = fk1;

                    mu = mu.Value * Math.Max(1.0 / 3.0, 1.0 - Math.Pow(2.0 * p - 1, 3));
                    upsilon = 2;

                    return true;
                }

                mu *= upsilon;
                upsilon *= 2;
            }
            return false;
        }
    }
}
