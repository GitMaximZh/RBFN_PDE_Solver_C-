using System;
using Common;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.Gradient
{
    public class QuickGradientMethod : BaseGradientMethod
    {
        public ISpeedFinder SpeedFinder { get; set; }

        public double Speed { get; set; }
        public double MinStep { get; set; }

        public QuickGradientMethod(IDifferentiableFunction function, Point startPoint)
            : base(function, startPoint)
        {
            SpeedFinder = new GoldenSectionSpeedFinder();
            SpeedFinder.MinStep = MinStep;
        }

        public override bool DoOptimizationStep()
        {
            var direction = -Function.Gradient(CurrentPoint);
            Speed = SpeedFinder.FindOptimalSpeed(Function, CurrentPoint, direction, Speed);
            if (Speed == 0)
                return false;
            var step = Speed * direction;
            CurrentPoint = CurrentPoint.Plus(step);
            CurrentValue = Function.Value(CurrentPoint);
            return true;
        }
    }
}
