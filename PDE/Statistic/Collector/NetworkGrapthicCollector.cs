using System.Collections.Generic;
using System.Linq;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class NetworkGrapthicCollector : GraphicCollector
    {
        private readonly IPoint[] _points;
        private readonly int cfg;

        public NetworkGrapthicCollector(IPoint[] points, int cfg)
        {
            _points = points;
            this.cfg = cfg;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            var network = cfg == 0 || cfg == 1 ? trainState.Problem.Network : null;
            var prevNetwork = cfg == 0 || cfg == 2 ? trainState.Problem.PreviousNetwork : null;
            for (int i = 0; i < _points.Length; i++)
                yield return new Point(_points[i].Coordinate
                    .Concat(new[] { (network != null ? network.Compute(_points[i].Coordinate) : 0.0) + (prevNetwork != null ? prevNetwork.Compute(_points[i].Coordinate) : 0.0) }).ToArray());
        }
    }
}
