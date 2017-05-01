using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE
{
    public class F1tBoundControlPoint : ControlPoint<Problem>
    {
        public F1tBoundControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            Tag = 4;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return Math.Cos(this[1]);
        }

        public override object Clone()
        {
            return new F1tBoundControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
