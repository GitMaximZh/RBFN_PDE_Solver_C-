using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem2.Inverse
{
    public class InnerControlPoint : ControlPoint<CoefficientProblem>
    {
        public InnerControlPoint(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var d2ud2x1 = Problem.Network.SecondDerivation(Coordinate, 0);
            var d2ud2x2 = Problem.Network.SecondDerivation(Coordinate, 1);
            return  -d2ud2x1 - d2ud2x2 + Problem.Approximator.Value(new [] { Coordinate[1] }) 
                * Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
