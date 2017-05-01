using OptimizationToolbox;
using OptimizationToolbox.QuasiNewton;
using PDE;
using PDE.Function;
using PDE.Optimizer;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PoissonPDE.QuasiNewton
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

        public OptimizationMethod CreateOptimizeMethod(Problem problem, IErrorFunctional functional, 
            IControlPoint[] controlPoints, ProblemParameters parametersToOptimize, bool optimizationProblemChanged, 
            OptimizationMethod predecessor = null)
        {
            var errorFunction = new JacobianErrorFunction(problem, controlPoints, functional, parametersToOptimize);
            var method = new LevenbergMarquardtMethod(errorFunction,
                problem.GetPoint(parametersToOptimize));
            method.MinStep = MinStep;
            method.Tau = Tau;
            return method;
        }
    }
}
