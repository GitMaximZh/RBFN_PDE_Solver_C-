using System;
using Common;
using RBFNetwork.Train;

namespace WavePDE
{
    internal class F0tBoundControlPoint : ControlPoint
    {
        public F0tBoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            Tag = 3;
        }

        public override double ApproximateValue(Func<Point, double> approximator)
        {
            return approximator(this);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new F0tBoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
