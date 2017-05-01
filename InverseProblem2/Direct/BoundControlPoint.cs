using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem2.Direct
{
    public class BoundControlPoint : ControlPoint<Problem>
    {
        public BoundControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }
        
        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return 1.0 + Coordinate[0];
        }

        public override object Clone()
        {
            return new BoundControlPoint(Problem, Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
