using RBFNetwork.Train.ErrorFunctional;

namespace RBFNetwork.Train
{
    public interface IOptimizer
    {
        bool ShouldBeStoped(int step, double error, double previousError);
        double Optimize(Problem problem, IErrorFunctional functional,
                      IControlPoint[] controlPoints, int step, double error, bool optimizationProblemChanged);
    }
}
