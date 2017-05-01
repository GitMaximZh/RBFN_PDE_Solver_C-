using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE
{
    public class InnerControlPoint : ControlPoint<Problem>
    {
        public InnerControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            Tag = 0;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.SecondDerivation(this.Coordinate, 1)
                - Problem.Network.SecondDerivation(this.Coordinate, 0) + Math.Pow(Problem.Network.Value(this.Coordinate), 2);
        }

        public override double ExpectedValue()
        {
            return -this[0] * Math.Cos(this[1]) + Math.Pow(this[0], 2) * Math.Pow(Math.Cos(this[1]), 2);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
 }
