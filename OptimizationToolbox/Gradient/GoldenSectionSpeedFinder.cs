using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.Gradient
{
    public class GoldenSectionSpeedFinder : ISpeedFinder
    {
        public double MinStep { get; set; }
        public double MaxImprovementSteps { get; set; }

        public GoldenSectionSpeedFinder()
        {
            MaxImprovementSteps = 10;
        }

        public double FindOptimalSpeed(IFunction function, Point point, DenseVector direction, double initialSpeed)
        {
            Func<double, bool> speedIsTooSmall = speed => (direction*speed).Norm(2) < MinStep;
            if (speedIsTooSmall(initialSpeed))
                return 0;

            var initialValue = function.Value(point);
            var fa = initialValue;
           

            var a = 0.0;
            var b = initialSpeed;

            double fb;

            //forward steps
            while (true)
            {
                fb = function.Value(point.Plus(b*direction));
                if (fb >= fa)
                    break;

                a = b;
                fa = fb;
                b *= 2;
            }
            
            //inner optimization
            for (int i = 0; i < MaxImprovementSteps; i++)
            {
                var delta = 0.5 * (b - a);
                var fab = function.Value(point.Plus((a + delta) * direction));

                if (fb >= fa)
                {
                    b = a + delta;
                    fb = fab;

                    if (speedIsTooSmall(b))
                        return 0;
                }
                else
                {
                    a = a + delta;
                    fa = fab;
                }
            }

            if (fa < fb && speedIsTooSmall(a))
                return 0;

            return fa < fb ? a : b;
        }
    }
}
