using Common;
using OptimizationToolbox.Common;

namespace OptimizationToolbox
{
    public abstract class OptimizationMethod
    {
        public IFunction Function { get; private set; }
        protected Point StartPoint { get; private set; }

        public Point CurrentPoint { get; protected set; }
        public double? CurrentValue { get; protected set; }

        protected bool breakStep = false;

        public OptimizationMethod(IFunction function, Point startPoint)
        {
            Function = function;
            StartPoint = startPoint;

            CurrentPoint = StartPoint;
            CurrentValue = null;
        }

        public abstract bool DoOptimizationStep();

        public void BreakStep()
        {
            breakStep = true;
        }
    }
}
