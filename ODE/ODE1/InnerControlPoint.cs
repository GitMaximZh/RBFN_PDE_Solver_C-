using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;
using PDE;

namespace ODE.ODE1
{
    public class InnerControlPoint : ControlPoint
    {
        public InnerControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            var d2 = f.SecondDerivation(Coordinate, 0);
            var d1 = f.FirstDerivation(Coordinate, 0);
            return d2 - 2.0 * d1 + f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return this[0] - 1.0;
        }

        public override object Clone()
        {
            return new InnerControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
