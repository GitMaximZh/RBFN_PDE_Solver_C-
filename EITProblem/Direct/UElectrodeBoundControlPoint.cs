using System;
using System.Collections.Generic;
using System.Linq;
using EITProblem.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Direct
{
    public class UElectrodeBoundControlPoint : ControlPoint<Problem>
    {
        private Electrode electrode;
        private double length;

        public UElectrodeBoundControlPoint(Problem problem, double weight, Electrode electrode, params double[] coordinate)
            : base(problem, weight, coordinate)
        {
            this.electrode = electrode;
            this.length = 1.0; // electrode.Length / electrode.Density;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            return length * Problem.Network.Compute(Coordinate);
        }

        public override double ExpectedValue()
        {
            var values = new List<double>();
            foreach (var point in electrode)
                values.Add(Problem.Network.Compute(point.Coordinate));
            return length * (electrode.CumValue > 0 ? values.Max() : values.Min());
        }

        public override object Clone()
        {
            return new UElectrodeBoundControlPoint(Problem, Weight, electrode, Coordinate) { Tag = Tag, Index = Index };
        }
    }
}
