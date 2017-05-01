using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem3.Inverse
{
    public class PositiveCondition : ControlPoint<CoefficientProblem>
    {
        public PositiveCondition(CoefficientProblem problem, double weight, 
            params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            
        }
        
        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var ku = Problem.Approximator.Value(Coordinate);
            return ku - Math.Abs(ku);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new PositiveCondition(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
