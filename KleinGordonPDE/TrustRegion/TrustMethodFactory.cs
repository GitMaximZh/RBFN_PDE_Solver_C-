using OptimizationToolbox.TrustRegion;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace KleinGordonPDE.TrustRegion
{
    public class TrustMethodFactory : IOpimizationMethodFactory
    {
        public bool PGP { get; set; }

        public OptimizationToolbox.OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional,
            IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged, OptimizationToolbox.OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianTwiceDifferentiableErrorFunction(problem, controlPoints, functional, parametersToOptimize);
            var method = new TrustRegionMethod(errorFunction,
                problem.GetPoint(parametersToOptimize), PGP ? (ISubProblemResolver)new PCGResolver() : 
                new CGResolver());
            method.TrustRegion = 2;
            //method.MinTrustRegion = 0.000001;
            //method.Mu2 = 0.4;
            //method.Mu1 = 0.9;
            return method;
        }
    }
}
