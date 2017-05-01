using System;
using Common;
using RBFNetwork.Train;

namespace WavePDE
{
    internal class Fx0BoundControlPoint : ControlPoint
    {
        public Fx0BoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            Tag = 1;
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
            return new Fx0BoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
