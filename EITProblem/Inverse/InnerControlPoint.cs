using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Inverse
{
    public class InnerControlPoint : ControlPoint<CoefficientProblem>
    {
        public InnerControlPoint(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var rx = Problem.Approximator.Compute(Coordinate);

            var dudx1 = Problem.Network.FirstDerivation(Coordinate, 0);
            var d2ud2x1 = Problem.Network.SecondDerivation(Coordinate, 0);
            var drdx1 = Problem.Approximator.FirstDerivation(Coordinate, 0);

            var dudx2 = Problem.Network.FirstDerivation(Coordinate, 1);
            var d2ud2x2 = Problem.Network.SecondDerivation(Coordinate, 1);
            var drdx2 = Problem.Approximator.FirstDerivation(Coordinate, 1);
            return drdx1 * dudx1 + rx * d2ud2x1 + drdx2 * dudx2 + rx * d2ud2x2;
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
