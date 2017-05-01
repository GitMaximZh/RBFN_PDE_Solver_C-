using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class IterationInfoCollector : StatisticCollector
    {
        public int CurrentIteration { get; private set; }
        public double CurrentError { get; private set; }

        public override void Collect(Trainer.TrainStateArg iteration)
        {
            CurrentIteration = iteration.Iteration;
            CurrentError = iteration.Error;
        }
    }
}
