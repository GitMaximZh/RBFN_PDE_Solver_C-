using System.Collections.Generic;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public abstract class GraphicCollector : StatisticCollector
    {
        public IEnumerable<Point> Points { get; private set; }

        public override void Collect(Trainer.TrainStateArg trainState)
        {
            Points = BuildGrapthic(trainState);
        }

        protected abstract IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState);
    }
}
