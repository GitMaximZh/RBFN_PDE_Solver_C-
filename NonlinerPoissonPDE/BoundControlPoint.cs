using Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace NonlinerPoissonPDE
{
    public class BoundControlPoint : ControlPoint
    {
        public BoundControlPoint(double weight, Point point)
            :base(weight, point)
        {
        }

        public BoundControlPoint(double weight, params double[] coordinate)
            : base(weight, coordinate)
        {
        }

        public BoundControlPoint(double weight, int dimention = 1)
            : base(weight, dimention)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction f)
        {
            return f.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return 0;
        }

        public override object Clone()
        {
            return new BoundControlPoint(Weight, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
