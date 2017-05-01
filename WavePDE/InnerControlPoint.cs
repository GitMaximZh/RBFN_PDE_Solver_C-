using System;
using Common;
using PDE;
using RBFNetwork.Train;

namespace WavePDE
{
    internal class InnerControlPoint : ControlPoint
    {
        public InnerControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            Tag = 0;
        }

        public override double ApproximateValue(Func<Point, double> approximator)
        {
            var d2udx2 = NumericTools.SecondOrderDerivation(approximator, this, 0);
            var d2udt2 = NumericTools.SecondOrderDerivation(approximator, this, 1);
            return d2udt2 - d2udx2;
        }

        public override double ExpectedValue()
        {
            return Math.Pow(Math.PI, 2) * Math.Sin(Math.PI * this[1]) * (1 - 2 * this[0] / 1.0);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
