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
    public class IEqualityCheckPoint : ControlPoint<Problem>
    {
        private Electrode e1;
        private Electrode e2;


        public IEqualityCheckPoint(Problem problem, double weight,
            Electrode e1, Electrode e2)
            : base(problem, weight, 2)
        {
            this.e1 = e1;
            this.e2 = e2;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var sum = 0.0;
            double? a = null; double b;
            var step = e1.Length / e1.Density;

            foreach (var point in e1)
            {
                b = DirectSuite.Resistency(point.Coordinate) *
                    (e1.Normal[0] * Problem.Network.FirstDerivation(point.Coordinate, 0) +
                     e1.Normal[1] * Problem.Network.FirstDerivation(point.Coordinate, 1));
                if (!a.HasValue)
                {
                    a = b;
                    continue;
                }
                sum += 0.5 * step * (a.Value + b);
                a = b;
            }

            a = null;
            step = e2.Length / e2.Density;
            foreach (var point in e2)
            {
                b = DirectSuite.Resistency(point.Coordinate) *
                    (e2.Normal[0] * Problem.Network.FirstDerivation(point.Coordinate, 0) +
                     e2.Normal[1] * Problem.Network.FirstDerivation(point.Coordinate, 1));
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
            return 0;
        }

        public override object Clone()
        {
            return new IEqualityCheckPoint(Problem, Weight, e1, e2) { Tag = Tag, Index = Index };
        }
    }
}
