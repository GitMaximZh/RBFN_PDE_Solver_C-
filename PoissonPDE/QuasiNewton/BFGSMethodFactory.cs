using OptimizationToolbox;
using OptimizationToolbox.Gradient;
using OptimizationToolbox.QuasiNewton;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PoissonPDE.QuasiNewton
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

        public OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional, 
            IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged, 
            OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianDifferentiableErrorFunction(problem, controlPoints, functional, parametersToOptimize);
            var method = new BFGSMethod(errorFunction,
                problem.GetPoint(parametersToOptimize));
            method.MinStep = MinStep;
            method.Speed = Speed;
            return method;
        }
    }
}
