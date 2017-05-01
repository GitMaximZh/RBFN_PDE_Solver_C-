using OptimizationToolbox;
using OptimizationToolbox.Gradient;
using OptimizationToolbox.QuasiNewton;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace WavePDE.QuasiNewton
{
    internal class BFGSMethodFactory : IOpimizationMethodFactory
    {
        public double MinStep { get; set; }
        public double Speed { get; set; }

        public BFGSMethodFactory()
        {
            MinStep = 0.00001;
            Speed = 1;
        }

        public OptimizationMethod CreateOptimizeMethod(RBFNetwork.RBFNetwork network, IErrorFunctional functional, 
            ControlPoint[] controlPoints, NetworkParameters parametersToOptimize, bool optimizationProblemChanged, 
            OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianDifferentiableErrorFunction(network, controlPoints, functional, parametersToOptimize);
            var method = new BFGSMethod(errorFunction,
                network.GetPoint(parametersToOptimize));
            method.MinStep = MinStep;
            method.Speed = Speed;
            return method;
        }
    }
}
