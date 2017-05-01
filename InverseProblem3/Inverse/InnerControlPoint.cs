using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem3.Inverse
{
    public class InnerControlPoint : ControlPoint<CoefficientProblem>
    {
        public InnerControlPoint(CoefficientProblem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var dudt = Problem.Network.FirstDerivation(Coordinate, 1);
            
            var dudx = Problem.Network.FirstDerivation(Coordinate, 0);
            var d2ud2x2 = Problem.Network.SecondDerivation(Coordinate, 0);

            var u = Problem.Network.Value(Coordinate);
            var ku = Problem.Approximator.Value(new[] { u });
            var dku = Problem.Approximator.FirstDerivation(new[] { u }, 0);

            return dudt - dku * dudx * dudx - ku * d2ud2x2;
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
