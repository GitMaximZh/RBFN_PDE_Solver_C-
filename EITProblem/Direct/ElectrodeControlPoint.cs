using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using EITProblem.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Direct
{
    public class ElectrodeControlPoint : ControlPoint<Problem>
    {
        private Electrode electrode;

        public ElectrodeControlPoint(Problem problem, double weight, Electrode electrode)
            : base(problem, weight, 2)
        {
            this.electrode = electrode;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var sum = 0.0;
            var step = electrode.Length / electrode.Density;
            double? a = null;
            double b;
            foreach (var point in electrode)
            {
                b = DirectSuite.Resistency(point.Coordinate) *
                    (electrode.Normal[0] * Problem.Network.FirstDerivation(point.Coordinate, 0) +
                     electrode.Normal[1] * Problem.Network.FirstDerivation(point.Coordinate, 1));
                if (!a.HasValue)
                {
                    a = b;
                    continue;
                }
                sum += 0.5 * step * (a.Value + b);
                a = b;
            }

            return sum;
        }

        public override double ExpectedValue()
        {
            return electrode.CumValue;
        }

        public override object Clone()
        {
            return new ElectrodeControlPoint(Problem, Weight, electrode) { Tag = Tag, Index = Index };
        }
    }
}
