using RBFNetwork.Train;

namespace PDE.Solution
{
    public interface IControlPointsFactory
    {
        IControlPoint[] Create(Problem problem);
    }
}
