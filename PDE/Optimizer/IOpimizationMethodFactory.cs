using OptimizationToolbox;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Optimizer
{
    public interface IOpimizationMethodFactory
    {
        OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional,
                             IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged,
            OptimizationMethod predecessor = null);
    }
}
