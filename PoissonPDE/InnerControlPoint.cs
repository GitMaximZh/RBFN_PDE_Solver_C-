using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;
using PDE;

namespace PoissonPDE
{
    public class InnerControlPoint : ControlPoint<Problem>
    {
        private double prevResult = 0.0;

        public InnerControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            if (problem.PreviousNetwork != null)
            {
                var d2udx2 = problem.PreviousNetwork.SecondDerivation(Coordinate, 0);
                var d2udy2 = problem.PreviousNetwork.SecondDerivation(Coordinate, 1);
                prevResult = d2udx2 + d2udy2;
            }
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var d2udx2 = Problem.Network.SecondDerivation(Coordinate, 0);
            var d2udy2 = Problem.Network.SecondDerivation(Coordinate, 1);
            return d2udx2 + d2udy2 + prevResult;
        }

        public override double ExpectedValue()
        {
            return Math.Sin(Math.PI*this[0])*Math.Sin(Math.PI*this[1]);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
