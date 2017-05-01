using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem2.Inverse
{
    public class BoundControlPoint1 : ControlPoint<CoefficientProblem>
    {
        private static readonly Random Rand = new Random(15123);

        private int direction;
        private RBFNetwork.RBFNetwork directApproximation;
        private double expectedValue = 0.0;

        public BoundControlPoint1(CoefficientProblem problem, double weight, RBFNetwork.RBFNetwork directApproximation,
            int direction, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.direction = direction;
            this.directApproximation = directApproximation;
            expectedValue = directApproximation.FirstDerivation(Coordinate, direction) + 2 * 0.08 * (Rand.NextDouble() - 0.5);
        }
        
        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.FirstDerivation(Coordinate, direction);
        }

        public override double ExpectedValue()
        {
            return expectedValue;
        }

        public override object Clone()
        {
            return new BoundControlPoint1(Problem, Weight, directApproximation, direction, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
