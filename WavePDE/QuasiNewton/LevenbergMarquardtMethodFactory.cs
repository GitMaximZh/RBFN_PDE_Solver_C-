using OptimizationToolbox;
using OptimizationToolbox.QuasiNewton;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace WavePDE.QuasiNewton
{
    internal class LevenbergMarquardtMethodFactory : IOpimizationMethodFactory
    {
        public double MinStep { get; set; }
        public double Tau { get; set; }

        public LevenbergMarquardtMethodFactory()
        {
            MinStep = 0.00001;
            Tau = 0.1;
        }

        public OptimizationMethod CreateOptimizeMethod(RBFNetwork.RBFNetwork network, IErrorFunctional functional, 
            ControlPoint[] controlPoints, NetworkParameters parametersToOptimize, bool optimizationProblemChanged, 
            OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianErrorFunction(network, controlPoints, functional, parametersToOptimize);
            var method = new LevenbergMarquardtMethod(errorFunction,
                network.GetPoint(parametersToOptimize));
            method.MinStep = MinStep;
            method.Tau = Tau;
            return method;
        }
    }
}
