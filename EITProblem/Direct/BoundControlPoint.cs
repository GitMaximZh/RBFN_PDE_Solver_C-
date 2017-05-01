using System;
using MathNet.Numerics.LinearAlgebra.Double;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Direct
{
    public class BoundControlPoint : ControlPoint<Problem>
    {
        private double expectedValue;
        private Vector normal;

        public BoundControlPoint(Problem problem, double weight, Vector normal, double expectedValue, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.normal = normal;
            this.expectedValue = expectedValue;
        }
        
        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return DirectSuite.Resistency(Coordinate) * (normal[0] * Problem.Network.FirstDerivation(Coordinate, 0) + normal[1] * Problem.Network.FirstDerivation(Coordinate, 1));
            
        }

        public override double ExpectedValue()
        {
            return expectedValue;
        }

        public override object Clone()
        {
            return new BoundControlPoint(Problem, Weight, normal, expectedValue, Coordinate) {Tag = Tag, Index = Index};
        }
    }
}
