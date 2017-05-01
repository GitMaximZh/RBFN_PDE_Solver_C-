using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace TestProblem
{
    public class InnerControlPoint : ControlPoint<Problem>
    {
        public InnerControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var d1udt = Problem.Network.FirstDerivation(Coordinate, 2);
            var d1udx = Problem.Network.FirstDerivation(Coordinate, 0);
            var d1udz = Problem.Network.FirstDerivation(Coordinate, 1);
            var d2udz2 = Problem.Network.SecondDerivation(Coordinate, 1);
            return d1udt + Suite.U1(Coordinate) * d1udx + 0.2 * d1udz + 0.2 * Coordinate[1] * d2udz2;
        }

        public override double ExpectedValue()
        {
            return Suite.F1(Coordinate);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
