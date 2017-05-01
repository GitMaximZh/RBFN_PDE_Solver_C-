using System;
using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace ODE.Function
{
    public class FunctionControlPoint : ControlPoint
    {
        private Func<double, double> func;

        public FunctionControlPoint(Func<double, double> func, double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
            this.func = func;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return func(this[0]);
        }

        public override object Clone()
        {
            return new FunctionControlPoint(func, Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
