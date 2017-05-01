using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;
using OptimizationToolbox.Gradient;

namespace OptimizationToolbox.QuasiNewton
{
    public class BFGSMethod : OptimizationMethod
    {
        public ISpeedFinder SpeedFinder { get; set; }

        public double Speed { get; set; }
        public double MinStep { get; set; }

        public new IDifferentiableFunction Function { get { return (IDifferentiableFunction)base.Function; } }

        private DenseVector _gk;
        private DenseMatrix _Vk;

        public BFGSMethod(IDifferentiableFunction function, Point startPoint)
            : base(function, startPoint)
        {
            SpeedFinder = new GoldenSectionSpeedFinder();
            SpeedFinder.MinStep = MinStep;

            _gk = function.Gradient(CurrentPoint);
            _Vk = new DenseMatrix(DiagonalMatrix.Identity(CurrentPoint.Dimension));
        }

        public override bool DoOptimizationStep()
        {
            var pk = -1 * _Vk * _gk;
            Speed = SpeedFinder.FindOptimalSpeed(Function, CurrentPoint, pk, Speed);
            if (Speed == 0)
                return false;

            var sk = Speed * pk;
            CurrentPoint = CurrentPoint.Plus(sk);
            CurrentValue = Function.Value(CurrentPoint);

            var gk1 = Function.Gradient(CurrentPoint);
            var yk = gk1 - _gk;

            var mYk = new DenseMatrix(yk.Count, 1, yk.ToArray());
            var mSk = new DenseMatrix(sk.Count, 1, sk.ToArray());

            var sk_yk = sk * yk;

            var Vk1 = _Vk + (sk_yk + (mYk.Transpose() * _Vk * mYk)[0, 0]) / Math.Pow(sk_yk, 2) *
                      (mSk * mSk.Transpose()) - 1 / sk_yk * (_Vk * mYk * mSk.Transpose() +
                      mSk * mYk.Transpose() * _Vk);

            _gk = gk1;
            _Vk = new DenseMatrix(Vk1);

            return true;
        }
    }
}
