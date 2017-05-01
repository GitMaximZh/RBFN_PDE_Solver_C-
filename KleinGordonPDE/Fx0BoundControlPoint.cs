using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE
{
    public class Fx0BoundControlPoint : ControlPoint<Problem>
    {
        private double expectedValue;
        public Fx0BoundControlPoint(Problem problem, double weight, double expectedValue,  params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.expectedValue = expectedValue;
            Tag = 1;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return expectedValue;
        }

        public override object Clone()
        {
            return new Fx0BoundControlPoint(Problem, Weight, expectedValue, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
