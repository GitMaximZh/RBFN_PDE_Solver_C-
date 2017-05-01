using System;
using System.Collections.Generic;
using RBFNetwork.Common;
using RBFNetwork.Function;

namespace RBFNetwork
{
    public class RBFNetwork : ITwiceDifferentiableFunction
    {
        public int InputCount { get; private set; }
        public int HiddenCount { get { return Functions.Length; } }
        public int OutputCount { get { return 1; } }

        private List<IBasisFunction> functions;
        public IBasisFunction[] Functions { get { return functions.ToArray(); } }

        private List<Parameter> weights;
        public Parameter[] Weights { get { return weights.ToArray(); } }
        
        public RBFNetwork(int inputCount, int functionsCount, Func<IBasisFunction> creator)
        {
            InputCount = inputCount;
            functions = new List<IBasisFunction>();

            for (int i = 0; i < functionsCount; i++)
                functions.Add(creator());

            weights = new List<Parameter>();
            for (int i = 0; i < functionsCount; i++)
                weights.Add(new Parameter());
        }

        public void AddFunction(Parameter weight, IBasisFunction function)
        {
            weights.Add(weight);
            functions.Add(function);
        }

        public virtual double Compute(double[] input)
        {
            double value = 0.0;
            for (int i = 0; i < Functions.Length; i++)
            {
                value += Weights[i] * Functions[i].Calculate(input);
            }
            return value;
        }

        public virtual void Undo()
        {
            foreach (var w in Weights)
                w.Undo();
            foreach (var f in Functions)
            {
                foreach (var p in f.Parameters)
                    p.Undo();
            }
        }

        public double Value(double[] x)
        {
            return Compute(x);
        }

        public virtual double FirstDerivation(double[] x, int dim)
        {
            double value = 0.0;
            for (int i = 0; i < Functions.Length; i++)
            {
                value += Weights[i] * Functions[i].FirstDerivation(x, dim);
            }
            return value;
        }

        public virtual double SecondDerivation(double[] x, int dim)
        {
            double value = 0.0;
            for (int i = 0; i < Functions.Length; i++)
            {
                value += Weights[i] * Functions[i].SecondDerivation(x, dim);
            }
            return value;
        }
    }
}
