using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EvolutionInverseProblem.Direct
{
    public class BoundControlPoint : ControlPoint<Problem>
    {
        private double expectedValue;
        public BoundControlPoint(Problem problem, double weight, double expectedValue, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.expectedValue = expectedValue;
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
            return new BoundControlPoint(Problem, Weight, expectedValue, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
