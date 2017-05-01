using OptimizationToolbox;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Optimizer
{
    public abstract class SubOptimizer : ISubOptimizer
    {
        public ProblemParameters ParametersToOptimize { get; set; }

        private readonly IOpimizationMethodFactory _opimizationMethodFactory;
        protected OptimizationMethod _optimizeMethod;
        
        public SubOptimizer(IOpimizationMethodFactory opimizationMethodFactory)
        {
            ParametersToOptimize = new ProblemParameters();

            _opimizationMethodFactory = opimizationMethodFactory;
        }

        public bool Optimize(Problem problem, IErrorFunctional functional,
                             IControlPoint[] controlPoints, int step, ref double error, bool optimizationProblemChanged)
        {
            if (optimizationProblemChanged)
                _optimizeMethod = null;
            if (!ShouldOptimize(step))
                return false;
            if (_optimizeMethod == null)
                _optimizeMethod = _opimizationMethodFactory.CreateOptimizeMethod(problem, functional, controlPoints,
                                                                                 ParametersToOptimize, optimizationProblemChanged, _optimizeMethod);

            if (_optimizeMethod.DoOptimizationStep())
            {
                error = _optimizeMethod.CurrentValue.Value;
                problem.MoveToPoint(_optimizeMethod.CurrentPoint, ParametersToOptimize);
                problem.ClearHistory(ParametersToOptimize);
                return true;
            }
            return false;
        }
        
        protected abstract bool ShouldOptimize(int step);
        public abstract void OnNetworkChanged();
    }
}
