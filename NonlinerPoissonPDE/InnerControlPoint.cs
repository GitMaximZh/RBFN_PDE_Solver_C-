using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;
using PDE;

namespace NonlinerPoissonPDE
{
    public class InnerControlPoint : ControlPoint
    {
        public InnerControlPoint(double weight, Point point)
            :base(weight, point)
        {
        }

        public InnerControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public InnerControlPoint(double weight, int dimention = 1)
            : base(weight, dimention)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return -1.0 * (f.SecondDerivation(Coordinate, 0) + f.SecondDerivation(Coordinate, 1)) - Math.PI * Math.PI * f.Value(Coordinate) *
                (1.0 - f.Value(Coordinate)) / 4;
        }

        public override double ExpectedValue()
        {
            var y = Math.PI*this[1]/2.0;
            var x = this[0];
            return 2.0*Math.Sin(y) + Math.PI*Math.PI/4.0*Math.Pow(1 - x*x, 2)*Math.Pow(Math.Sin(y), 2);
        }

        public override object Clone()
        {
            return new InnerControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
