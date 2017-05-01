using System.Collections.Generic;
using Common;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Solution
{
    public class TrainFactory : ITrainerFactory
    {
        private readonly IOptimizer _optimizer;
        private readonly IControlPointsTransformer _transformer;
        
        public TrainFactory(IOptimizer optimizer, IControlPointsTransformer transformer = null)
        {
            _optimizer = optimizer;
            _transformer = transformer;
        }

        public Trainer Create(Problem problem, IControlPoint[] points)
        {
            var trainer = new Trainer(problem, new QuadraticErrorFunctional(), points);
            trainer.Optimizer = _optimizer;
            trainer.ContolPointsTransformer = _transformer;
            return trainer;
        }
    }
}
