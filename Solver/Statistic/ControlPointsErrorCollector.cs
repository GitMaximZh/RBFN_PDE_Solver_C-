using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDE.Statistic;
using RBFNetwork.Train;

namespace Solver.Statistic
{
    public class ControlPointsErrorCollector : StatisticCollector
    {
        private readonly IControlPoint[] _controlPoints;

        public double Error { get; private set; }
    
        public ControlPointsErrorCollector(IEnumerable<IControlPoint> points)
        {
            _controlPoints = points.ToArray();
        }
        
        public override void Collect(Trainer.TrainStateArg iteration)
        {
            var error = 0.0;
            foreach (var p in _controlPoints)
            {
                double pReal = p.ApproximateValue();
                double pExact = p.ExpectedValue();
                error += Math.Pow(pReal - pExact, 2);
            }

            Error = error;
        }
    }
}
