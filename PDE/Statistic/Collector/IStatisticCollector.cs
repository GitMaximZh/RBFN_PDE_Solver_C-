using RBFNetwork.Train;

namespace PDE.Statistic
{
    public interface IStatisticCollector
    {
        void Collect(Trainer.TrainStateArg trainState);
    }
}
