
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace TestProblem
{
    public class InitialIntensityControlPoint : ControlPoint<Problem>
    {
        public InitialIntensityControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Value(Coordinate);
        }

        public override double ExpectedValue()
        {
            return Suite.I0(Coordinate);
        }

        public override object Clone()
        {
            return new InitialIntensityControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
