using System;
using MathNet.Numerics.LinearAlgebra.Double;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Direct
{
    public class IElectrodeBoundControlPoint : ControlPoint<Problem>
    {
        private double expectedValue;
        private double length;
        private Vector normal;

        public IElectrodeBoundControlPoint(Problem problem, double weight, Vector normal, double expectedValue, double length, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.normal = normal;
            this.expectedValue = expectedValue;
            this.length = length;
        }
        
        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return length * DirectSuite.Resistency(Coordinate) * (normal[0] * Problem.Network.FirstDerivation(Coordinate, 0) + normal[1] * Problem.Network.FirstDerivation(Coordinate, 1));
            
        }

        public override double ExpectedValue()
        {
            return length * expectedValue;
        }

        public override object Clone()
        {
            return new IElectrodeBoundControlPoint(Problem, Weight, normal, expectedValue, length, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
