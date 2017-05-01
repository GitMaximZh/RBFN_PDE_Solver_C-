using System;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EvolutionInverseProblem.Direct
{
    public class InnerControlPoint : ControlPoint<Problem>
    {
        public InnerControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var dudt = Problem.Network.FirstDerivation(Coordinate, 1);
            var d2ud2x2 = Problem.Network.SecondDerivation(Coordinate, 0);
            return dudt - d2ud2x2;
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
