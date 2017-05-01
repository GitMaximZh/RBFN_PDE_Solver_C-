using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Function;

namespace RBFNetwork
{
    public class NormalizedRBFNetwork : RBFNetwork
    {
        public const double h = 0.0001;

        public NormalizedRBFNetwork(int inputCount, int functionsCount, Func<IBasisFunction> creator) 
            : base(inputCount, functionsCount, creator)
        {
        }

        public override double Compute(double[] input)
        {
            double numerator = 0.0;
            double denominator = 0.0;
            for (int i = 0; i < Functions.Length; i++)
            {
                var f = Functions[i].Calculate(input);
                numerator += Weights[i] * f;
                denominator += f;
            }
            return numerator / denominator;
        }

        public override double FirstDerivation(double[] x, int dim)
        {
            var left = (double[])x.Clone();;

            left[dim] += h;

            return (Compute(left) - Compute(x)) / h;
        }

        public override double SecondDerivation(double[] x, int dim)
        {
            var left = (double[])x.Clone();
            var right = (double[])x.Clone();

            left[dim] += h;
            right[dim] -= h;

            return (Compute(left) - 2 * Compute(x) + Compute(right)) / (h*h);
        }
    }
}
