using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem1
{
    public class InnerControlPoint : ControlPoint<CoefficientProblem>
    {
        public InnerControlPoint(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var dkdx = Problem.Approximator.FirstDerivation(Coordinate, 0);
            var dudx = Problem.Network.FirstDerivation(Coordinate, 0);
            var d2udx = Problem.Network.SecondDerivation(Coordinate, 0);
            var kx = Problem.Approximator.Value(Coordinate);
            return  dkdx * dudx + kx * d2udx;
                //dkdx * 3 * Coordinate[0] * Coordinate[0] + kx * 6 * Coordinate[0];
        }

        public override double ExpectedValue()
        {
            return 3;
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
