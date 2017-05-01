using Common;
using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.Common
{
    public interface ITwiceDifferentiableFunction : IDifferentiableFunction
    {
        DenseMatrix Hessian(Point point);
    }
}
