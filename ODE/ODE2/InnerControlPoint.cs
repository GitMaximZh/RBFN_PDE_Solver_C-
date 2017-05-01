using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;
using PDE;

namespace ODE.ODE2
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
            return d2 + 16.0 * f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return 2.0 * Math.Pow(Math.Cos(this[0]), 2);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
