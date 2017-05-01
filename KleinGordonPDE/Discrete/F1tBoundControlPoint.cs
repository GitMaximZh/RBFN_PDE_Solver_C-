using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE.Discrete
{
    public class F1tBoundControlPoint : ControlPoint<Problem>
    {
        private double t;
        public F1tBoundControlPoint(Problem problem, double t, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            Tag = 4;
            this.t = t;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var f = func ?? Problem.Network;
            return f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return Math.Cos(t);
        }

        public override object Clone()
        {
            return new F1tBoundControlPoint(Problem, t, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
