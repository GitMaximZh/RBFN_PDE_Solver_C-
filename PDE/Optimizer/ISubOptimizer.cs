using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Optimizer
{
    public interface ISubOptimizer
    {
        void OnNetworkChanged();

        bool Optimize(Problem problem, IErrorFunctional functional,
                      IControlPoint[] controlPoints, int step, ref double error, bool optimizationProblemChanged);
        
    }
}
