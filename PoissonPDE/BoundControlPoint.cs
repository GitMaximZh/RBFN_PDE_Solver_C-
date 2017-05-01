using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace PoissonPDE
{
    public class BoundControlPoint : ControlPoint<Problem>
    {
        private double prevResult = 0.0;

        public BoundControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            if (problem.PreviousNetwork != null)
                prevResult = problem.PreviousNetwork.Value(Coordinate);
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate) + prevResult;
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new BoundControlPoint(Problem, Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
