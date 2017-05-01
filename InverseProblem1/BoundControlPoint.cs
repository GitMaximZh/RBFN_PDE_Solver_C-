using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem1
{
    public class BoundControlPoint : ControlPoint<CoefficientProblem>
    {
        public BoundControlPoint(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return Math.Pow(Coordinate[0], 3);
        }

        public override object Clone()
        {
            return new BoundControlPoint(Problem, Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
