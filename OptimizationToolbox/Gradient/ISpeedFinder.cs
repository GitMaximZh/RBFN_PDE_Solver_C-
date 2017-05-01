using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.Gradient
{
    public interface ISpeedFinder
    {
        double MinStep { get; set; }
        double FindOptimalSpeed(IFunction function, Point point, DenseVector direction, double initialSpeed);
    }
}
