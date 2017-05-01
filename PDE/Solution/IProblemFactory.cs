using RBFNetwork.Train;

namespace PDE.Solution
{
    public interface IProblemFactory
    {
        Problem Create();
    }
}
