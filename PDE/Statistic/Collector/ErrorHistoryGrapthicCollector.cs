using System.Collections.Generic;
using Common;
using System.Linq;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class ErrorHistoryGrapthicCollector : GraphicCollector
    {
        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            return trainState.ErrorsHistory.Select((e, i) => new Point(i, e.Error));
        }
    }
}
