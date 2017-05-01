using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using RBFNetwork.Train;

namespace PDE.Optimizer
{
    public class CompositeOptimizer : IOptimizer
    {
        private readonly IStopStrategy _stopStrategy;
        private readonly List<ISubOptimizer> _strategies = new List<ISubOptimizer>();
        
        public CompositeOptimizer(IStopStrategy stopStrategy)
        {
            _stopStrategy = stopStrategy;
        }

        public void Add(ISubOptimizer strategy)
        {
            _strategies.Add(strategy);
        }

        public bool ShouldBeStoped(int step, double error, double previousError)
        {
            return _stopStrategy.ShouldBeStoped(step, error, previousError);
        }

        public double Optimize(Problem problem, RBFNetwork.Train.ErrorFunctional.IErrorFunctional functional, 
            IControlPoint[] controlPoints, int step, double error, bool optimizationProblemChanged)
        {
            foreach (var optimizer in _strategies)
            {
                if (optimizer.Optimize(problem, functional, controlPoints, step, ref error, optimizationProblemChanged))
                {
                    _strategies.Except(new[] {optimizer}).ToList().ForEach(s => s.OnNetworkChanged());
                }
            }
            return error;
        }
    }
}
