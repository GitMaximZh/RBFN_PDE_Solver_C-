using Common;
using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.Common
{
    public interface IDifferentiableFunction : IFunction
    {
        DenseVector Gradient(Point point);
    }
}
