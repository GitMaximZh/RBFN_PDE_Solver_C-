using Common;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.Gradient
{
    public abstract class BaseGradientMethod : OptimizationMethod
    {
        public new IDifferentiableFunction Function { get { return (IDifferentiableFunction)base.Function; } }

        protected BaseGradientMethod(IDifferentiableFunction function, Point startPoint)
            : base(function, startPoint)
        {

        }
    }
}
