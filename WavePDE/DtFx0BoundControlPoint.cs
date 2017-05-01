using System;
using Common;
using RBFNetwork.Train;
using PDE;

namespace WavePDE
{
    internal class DtFx0BoundControlPoint : ControlPoint
    {
        public DtFx0BoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            Tag = 2;
        }

        public override double ApproximateValue(Func<Point, double> approximator)
        {
            return NumericTools.FirstOrderDerivation(approximator, this, 1);
        }

        public override double ExpectedValue()
        {
            return Math.PI * Math.Cos(Math.PI * this[0]) - Math.PI * (1 - 2 * this[0] / 1.0);
        }

        public override object Clone()
        {
            return new DtFx0BoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
