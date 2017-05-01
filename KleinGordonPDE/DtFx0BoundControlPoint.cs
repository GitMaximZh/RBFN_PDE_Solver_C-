using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE
{
    public class DtFx0BoundControlPoint : ControlPoint<Problem>
    {
        private double expectedValue;
        public DtFx0BoundControlPoint(Problem problem, double weight, double expectedValue, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.expectedValue = expectedValue;
            Tag = 2;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.FirstDerivation(Coordinate, 1);
        }

        public override double ExpectedValue()
        {
            return expectedValue;
        }

        public override object Clone()
        {
            return new DtFx0BoundControlPoint(Problem, Weight, expectedValue, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
