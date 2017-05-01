namespace RBFNetwork.Function
{
    public interface ITwiceDifferentiableFunction
    {
        double Value(double[] x);
        double FirstDerivation(double[] x, int dim);
        double SecondDerivation(double[] x, int dim);
    }
}
