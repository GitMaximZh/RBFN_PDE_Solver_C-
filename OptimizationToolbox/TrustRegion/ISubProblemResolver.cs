using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.TrustRegion
{
    public interface ISubProblemResolver
    {
        DenseVector Resolve(SubProblem problem);
    }
}
