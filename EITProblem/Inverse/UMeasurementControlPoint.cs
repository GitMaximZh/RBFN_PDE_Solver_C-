using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EITProblem.Model;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Inverse
{
    public class UMeasurementControlPoint : ControlPoint<CoefficientProblem>
    {
        private Electrode e1;
        private Electrode e2;
        private double U;


        public UMeasurementControlPoint(CoefficientProblem problem, double weight,
            Electrode e1, Electrode e2, double U)
            : base(problem, weight, 2)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.U = U;
        }

        public override double ApproximateValue(ITwiceDifferentiableFunction func = null)
        {
            var sum = 0.0;
            double? a = null; double b;
            var step = e1.Length / e1.Density;

            foreach (var point in e1)
            {
                b = Problem.Network.Compute(point.Coordinate);
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
                b = Problem.Network.Compute(point.Coordinate);
                if (!a.HasValue)
                {
                    a = b;
                    continue;
                }
                sum -= 0.5 * step * (a.Value + b);
                a = b;
            }
            return sum;
        }

        public override double ExpectedValue()
        {
            return U;
        }

        public override object Clone()
        {
            return new UMeasurementControlPoint(Problem, Weight, e1, e2, U) { Tag = Tag, Index = Index };
        }
    }
}
