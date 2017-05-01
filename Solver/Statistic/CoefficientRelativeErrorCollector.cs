using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using PDE.Statistic;
using RBFNetwork.Train;

namespace Solver.Statistic
{
    public class CoefficientRelativeErrorCollector: StatisticCollector
    {
        private readonly IPoint[] _controlPoints;
        private readonly Func<IPoint, double> _expectedFunc;

        public double RelativeError { get; private set; }
    
        public CoefficientRelativeErrorCollector(Func<IPoint, double> expectedFunc)
            : this(expectedFunc, null)
        {
        }

        public CoefficientRelativeErrorCollector(Func<IPoint, double> expectedFunc, IPoint[] points)
        {
            _controlPoints = points;
            _expectedFunc = expectedFunc;
        }


        public override void Collect(Trainer.TrainStateArg iteration)
        {
            var approximator = ((CoefficientProblem)iteration.Problem).Approximator;
            var points = _controlPoints;

            var error = 0.0;
            var exact = 0.0;

            foreach (var point in points)
            {
                double pReal = approximator.Compute(point.Coordinate);
                double pExact = _expectedFunc(point);
                error += Math.Pow(pReal - pExact, 2);
                exact += Math.Pow(pExact, 2);
            }

            RelativeError = Math.Sqrt(error) / Math.Sqrt(exact);
        }
    }
}
