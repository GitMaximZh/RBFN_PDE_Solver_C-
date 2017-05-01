using OptimizationToolbox.TrustRegion;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace EvolutionInverseProblem.TrustRegion
{
    internal class TrustMethodFactory : IOpimizationMethodFactory
    {
        public OptimizationToolbox.OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional, 
            IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged, OptimizationToolbox.OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianTwiceDifferentiableErrorFunction(problem, controlPoints, functional, parametersToOptimize);
            var method = new TrustRegionMethod(errorFunction,
                problem.GetPoint(parametersToOptimize), new PCGResolver());
            method.TrustRegion = 2;
            return method;
        }
    }
}
