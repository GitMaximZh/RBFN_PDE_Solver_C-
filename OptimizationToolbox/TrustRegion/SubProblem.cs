using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.TrustRegion
{
    public class SubProblem
    {
        public DenseMatrix Hessian { get; set; }
        public DenseVector Gradient { get; set; }
        
        public double TrustRegion { get; set; }
    }
}
