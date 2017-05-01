using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem3.Inverse
{
    public class AdditionalControlPoint : ControlPoint<CoefficientProblem>
    {
        private static readonly Random Rand = new Random(15123);

        private RBFNetwork.RBFNetwork directApproximation;
        private double expectedValue;

        public AdditionalControlPoint(CoefficientProblem problem, double weight, RBFNetwork.RBFNetwork directApproximation,
            params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.directApproximation = directApproximation;
            expectedValue = directApproximation.Value(Coordinate) + 2 * 0.08 * (Rand.NextDouble() - 0.5);
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
            return new AdditionalControlPoint(Problem, Weight, directApproximation, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
