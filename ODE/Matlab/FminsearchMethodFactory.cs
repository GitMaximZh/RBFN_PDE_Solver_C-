using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptimizationToolbox.External;
using OptimizationToolbox.TrustRegion;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace ODE.Matlab
{
    public class FminsearchMethodFactory : IOpimizationMethodFactory
    {
        public OptimizationToolbox.OptimizationMethod CreateOptimizeMethod(RBFNetwork.RBFNetwork network, IErrorFunctional functional,
            ControlPoint[] controlPoints, NetworkParameters parametersToOptimize, bool optimizationProblemChanged, OptimizationToolbox.OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianTwiceDifferentiableErrorFunction(network, controlPoints, functional, parametersToOptimize);
            var method = new MatlabFminsearch(errorFunction,
                network.GetPoint(parametersToOptimize));
            return method;
        }
    }
}
