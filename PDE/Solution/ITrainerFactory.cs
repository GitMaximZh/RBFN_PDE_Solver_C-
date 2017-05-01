using RBFNetwork.Train;

namespace PDE.Solution
{
    public interface ITrainerFactory
    {
        Trainer Create(Problem problem, IControlPoint[] points);
    }
}
