using System;
using Common;
using PDE;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE.Discrete
{
    public class InnerControlPoint1 : ControlPoint<Problem>
    {
        private Problem pn;
        private Problem pn_1;
        private double t;
        private double dt;

        public InnerControlPoint1(Problem problem, Problem un, Problem un_1, double t, double dt, double weight, params double[] coordinate)
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
            var f = func ?? Problem.Network;
            var un1 = f.Value(Coordinate);
            var dun1 = f.SecondDerivation(Coordinate, 0);

            return un1 - 0.5 * dt * dt * dun1 + (pn == null && pn_1 == null ? un1 : 0);
        }

        public override double ExpectedValue()
        {
            var un = pn != null ? pn.Network.Compute(Coordinate) : this[0];
            var dun = pn != null ? pn.Network.SecondDerivation(Coordinate, 0) : 0;

            var un_1 = pn_1 != null ? pn_1.Network.Compute(Coordinate) : pn != null ? this[0] : 0;

            var fn1 = -this[0] * Math.Cos(t) + Math.Pow(this[0] * Math.Cos(t), 2);
            var fn = -this[0] * Math.Cos(t-dt) + Math.Pow(this[0] * Math.Cos(t-dt), 2);
            return 2.0 * un + 0.5 * dt * dt * dun - dt * dt * Math.Pow(un, 2) - un_1
                   + dt * dt * (0.5 *fn1 + 0.5 * fn);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Problem, pn, pn_1, t, dt, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
