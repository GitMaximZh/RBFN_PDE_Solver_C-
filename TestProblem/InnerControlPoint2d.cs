using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace TestProblem
{
    public class InnerControlPoint2d : ControlPoint<Problem>
    {
        public InnerControlPoint2d(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var d1udt = Problem.Network.FirstDerivation(Coordinate, 1);
            var u = Problem.Network.Compute(Coordinate);
            var d1udx = Problem.Network.FirstDerivation(Coordinate, 0);
            return d1udt + Suite2d.U1(Coordinate) * d1udx + Suite2d.Absorption * u;
        }

        public override double ExpectedValue()
        {
            return Suite2d.F(Coordinate);
        }

        public override object Clone()
        {
            return new InnerControlPoint2d(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
