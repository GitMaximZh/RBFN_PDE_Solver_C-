using RBFNetwork.Common;

namespace RBFNetwork.Function
{
    public interface IBasisFunction : ITwiceDifferentiableFunction
    {
        Parameter[] Center { get; set; }
        Parameter[] Parameters { get; set; }
        int Dimension { get; }
        
        double Calculate(double[] x);
    }
}
