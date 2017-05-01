using System;
using Common;
using PDE;
using RBFNetwork.Train;

namespace WavePDE
{
    internal class DxF0tBoundControlPoint : ControlPoint
    {
        public DxF0tBoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            Tag = 4;
        }

        public override double ApproximateValue(Func<Point, double> approximator)
        {
            var dxF0t = NumericTools.FirstOrderDerivation(approximator, this, 0);
            var dxF1t = NumericTools.FirstOrderDerivation(approximator, new Point(1, this[1]), 0);
            return dxF0t - dxF1t;
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new DxF0tBoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
