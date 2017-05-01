using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE.Discrete
{
    public class InnerControlPoint : ControlPoint<Problem>
    {
        private Problem pn;
        private Problem pn_1;
        private double t;
        private double dt;

        public InnerControlPoint(Problem problem, Problem un, Problem un_1, double t, double dt, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            Tag = 0;
            pn = un;
            pn_1 = un_1;
            this.t = t;
            this.dt = dt;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var un1 = Problem.Network.Compute(Coordinate);
            var dun1 = Problem.Network.SecondDerivation(Coordinate, 0);

            var un = pn != null ? pn.Network.Compute(Coordinate) : this[0];
            var dun = pn != null ? pn.Network.SecondDerivation(Coordinate, 0) : 0;

            var un_1 = pn_1 != null ? pn_1.Network.Compute(Coordinate) : pn != null ? this[0] : un1;

            var fn1 = -this[0] * Math.Cos(t) + Math.Pow(this[0] * Math.Cos(t), 2);
            var fn = -this[0] * Math.Cos(t - dt) + Math.Pow(this[0] * Math.Cos(t - dt), 2);

            return (un1 - 2.0 * un + un_1) / Math.Pow(dt, 2) + 0.5 * (-dun1 + Math.Pow(un1, 2) - fn1)
                   + 0.5 * (-dun + Math.Pow(un, 2) - fn);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, pn, pn_1, t, dt, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
