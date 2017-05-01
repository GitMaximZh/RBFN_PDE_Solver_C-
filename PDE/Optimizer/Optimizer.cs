using OptimizationToolbox;
using RBFNetwork;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Optimizer
{
    public class Optimizer : IOptimizer
    {
        public ProblemParameters ParametersToOptimize { get; set; }

        private OptimizationMethod _optimizeMethod;

        private readonly IStopStrategy _stopStrategy;
        private readonly IOpimizationMethodFactory _opimizationMethodFactory;

        public Optimizer(IStopStrategy stopStrategy, IOpimizationMethodFactory opimizationMethodFactory)
        {
            ParametersToOptimize = new ProblemParameters { NetworkParameters = new NetworkParameters(true, true, true) };

            _stopStrategy = stopStrategy;
            _opimizationMethodFactory = opimizationMethodFactory;
        }

        public bool ShouldBeStoped(int step, double error, double previousError)
        {
            return _stopStrategy.ShouldBeStoped(step, error, previousError);
        }

        public double Optimize(Problem problem, IErrorFunctional functional, 
            IControlPoint[] controlPoints, int step, double error, bool optimizationProblemChanged)
        {
            if (_optimizeMethod == null || optimizationProblemChanged)
                _optimizeMethod = _opimizationMethodFactory.CreateOptimizeMethod(problem, functional, controlPoints, 
                    ParametersToOptimize, optimizationProblemChanged, _optimizeMethod);
            
            if (_optimizeMethod.DoOptimizationStep())
            {
                problem.MoveToPoint(_optimizeMethod.CurrentPoint, ParametersToOptimize);
                problem.ClearHistory(ParametersToOptimize);
                return _optimizeMethod.CurrentValue.Value;
            }
            return error;
        }
    }
}
