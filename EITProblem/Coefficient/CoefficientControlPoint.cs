using System;
using EITProblem.Direct;
using MathNet.Numerics.LinearAlgebra.Double;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Coefficient
{
    public class CoefficientControlPoint : ControlPoint<Problem>
    {
        public CoefficientControlPoint(Problem problem, double weight, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return Problem.Network.Compute(Coordinate);
        }

        public override double ExpectedValue()
        {
            //var c = new[] { 0.3, 0.3 };
            //double r = 0.03;
            //return (new DenseVector(Coordinate) - new DenseVector(c)).Norm(2) > r ?
            //    1 : 0.2;
            
            //return 1.0;

            //var d = Math.Pow(Coordinate[0] - 0.5, 2) + Math.Pow(Coordinate[1] - 0.5, 2);
            //var d1 = Math.Pow(Coordinate[0] + 1, 2) + Math.Pow(Coordinate[1] + 1, 2);
            //return (d > 0.09 ? (d1 > 0.02 ? 1 : 1.1) : 0.2);
            //+ 1.7 * Math.Exp(-(Math.Pow(Coordinate[0] + 0.3, 2) / 0.2 + Math.Pow(Coordinate[1] + 0.5, 2) / 0.1));

            //for 2-d
            var dBgd = Math.Pow(Coordinate[0], 2) + Math.Pow(Coordinate[1], 2);
            var degree18 = 2.0 * Math.PI * -18 / 360;
            var dLung = Math.Pow((Coordinate[0] + 0.5) * Math.Cos(degree18) - (Coordinate[1]) * Math.Sin(degree18), 2) / Math.Pow(0.38, 2) +
                Math.Pow((Coordinate[0] + 0.5) * Math.Sin(degree18) + (Coordinate[1]) * Math.Cos(degree18), 2) / Math.Pow(0.625, 2);
            var dSpine = Math.Pow(Coordinate[0], 2) + Math.Pow(Coordinate[1]+0.65, 2);
            if (dLung <= 1)
                return 0.83;
            if (dSpine <= 0.0225)
                return 0.5;
            return 3.3;

        }

        public override object Clone()
        {
            return new CoefficientControlPoint(Problem, Weight, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
