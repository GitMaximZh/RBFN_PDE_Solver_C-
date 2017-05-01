using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace ODE.ODE1
{
    public class BoundControlPoint : ControlPoint
    {
        public BoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new BoundControlPoint(Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }

    public class DxBoundControlPoint : ControlPoint
    {
        public DxBoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return f.FirstDerivation(Coordinate, 0);
        }

        public override double ExpectedValue()
        {
            return -1;
        }

        public override object Clone()
        {
            return new DxBoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
