using System.Collections.Generic;
using Common;

namespace PDE.Statistic
{
    public class RBFGrapthicCollector : GraphicCollector
    {
        protected override IEnumerable<Point> BuildGrapthic(RBFNetwork.Train.Trainer.TrainStateArg trainState)
        {
            var network = trainState.Problem.Network;
            for (int i = 0; i < network.HiddenCount; i++)
            {
                var f = network.Functions[i];
                yield return new Point(f.Center[0], f.Center[1], f.Parameters[0], network.Weights[i]);
            }
        }
    }
}
