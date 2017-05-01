using OptimizationToolbox;
using OptimizationToolbox.Gradient;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PoissonPDE.Gradient
{
    internal class QuickGradientMethodFactory : IOpimizationMethodFactory
    {
        public double MinStep { get; set; }
        public double Speed { get; set; }

        public QuickGradientMethodFactory()
        {
            MinStep = 0.00001;
            Speed = 1;
        }

        public OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional, 
            IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged, 
            OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianDifferentiableErrorFunction(problem, controlPoints, functional, parametersToOptimize);
            var method = new QuickGradientMethod(errorFunction,
                problem.GetPoint(parametersToOptimize));
            method.MinStep = MinStep;
            if (predecessor != null)
                method.Speed = ((QuickGradientMethod)predecessor).Speed;
            else
                method.Speed = Speed;
            return method;
        }
    }
}
