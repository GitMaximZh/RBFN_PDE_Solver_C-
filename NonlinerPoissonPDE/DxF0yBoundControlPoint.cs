using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace NonlinerPoissonPDE
{
    public class DxF0yBoundControlPoint : ControlPoint
    {
        public DxF0yBoundControlPoint(double weight, Point point)
            :base(weight, point)
        {
        }

        public DxF0yBoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public DxF0yBoundControlPoint(double weight, int dimention = 1)
            : base(weight, dimention)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return f.FirstDerivation(Coordinate, 0);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new DxF0yBoundControlPoint(Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
