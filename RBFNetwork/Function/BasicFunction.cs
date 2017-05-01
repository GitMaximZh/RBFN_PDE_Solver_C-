using RBFNetwork.Common;
namespace RBFNetwork.Function
{
    public abstract class BasicFunction : IBasisFunction
    {
        public Parameter[] Center { get; set; }
        public Parameter[] Parameters { get; set; }
        public int Dimension { get; private set; }

        public BasicFunction(int dimension, int parameterCount)
        {
            Dimension = dimension;

            Center = new Parameter[Dimension];
            for (int i = 0; i < Dimension; i++)
                Center[i] = new Parameter();

            if (parameterCount != 0)
            {
                Parameters = new Parameter[parameterCount];
                for (int i = 0; i < parameterCount; i++)
                    Parameters[i] = new Parameter();
            }
        }

        public abstract double Calculate(double[] x);

        public double Value(double[] x)
        {
            return Calculate(x);
        }

        public abstract double FirstDerivation(double[] x, int dim);

        public abstract double SecondDerivation(double[] x, int dim);
    }
}
