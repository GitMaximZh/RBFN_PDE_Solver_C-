
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using PDE.Statistic;
using RBFNetwork.Train;

namespace Solver.Statistic
{
    public class RelativeErrorHistoryGraphicCollector : GraphicCollector
    {
        private readonly RelativeErrorCollector[] _collectors;

        private IDictionary<int, IList<double>> _errors = new Dictionary<int, IList<double>>(); 
        
        public RelativeErrorHistoryGraphicCollector(params RelativeErrorCollector[] collector)
        {
            _collectors = collector;
        }

        public override void Collect(Trainer.TrainStateArg trainState)
        {
            if(trainState.IsLastIteration)
                base.Collect(trainState);
            else
            {
                _collectors.ToList().ForEach(c => c.Collect(trainState));
                _errors.Add(trainState.Iteration, _collectors.Select(c => c.RelativeError).ToList());
            }
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            return _errors.Select(e => new Point(new double[] { 0.0, e.Key }.Concat(e.Value).ToArray()));
        }
    }
}
