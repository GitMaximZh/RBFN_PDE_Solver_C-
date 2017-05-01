using OptimizationToolbox.TrustRegion;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace ODE.TrustRegion
{
    internal class TrustMethodFactory : IOpimizationMethodFactory
    {
        public OptimizationToolbox.OptimizationMethod CreateOptimizeMethod(RBFNetwork.RBFNetwork network, IErrorFunctional functional, 
            ControlPoint[] controlPoints, NetworkParameters parametersToOptimize, bool optimizationProblemChanged, OptimizationToolbox.OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianTwiceDifferentiableErrorFunction(network, controlPoints, functional, parametersToOptimize);
            var method = new TrustRegionMethod(errorFunction,
                network.GetPoint(parametersToOptimize), new PCGResolver());
            method.TrustRegion = 2;
            return method;
        }
    }
}
