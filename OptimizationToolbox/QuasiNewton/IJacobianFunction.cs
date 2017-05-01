using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.QuasiNewton
{
    public interface  IJacobianFunction : IFunction
    {
        DenseMatrix Jacobian(Point point);
        DenseVector Difference(Point point);
    }
}
