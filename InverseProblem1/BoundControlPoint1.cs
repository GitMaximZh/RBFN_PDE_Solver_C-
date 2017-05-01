using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem1
{
    public class BoundControlPoint1 : ControlPoint<CoefficientProblem>
    {
        public BoundControlPoint1(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Approximator.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return Math.Pow(Coordinate[0], -1);
        }

        public override object Clone()
        {
            return new BoundControlPoint1(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
