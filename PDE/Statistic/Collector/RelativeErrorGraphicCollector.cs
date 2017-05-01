using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class RelativeErrorGraphicCollector : GraphicCollector
    {
        private readonly IPoint[] _points;
        private readonly Func<IPoint, double> _expectedFunc;

        public RelativeErrorGraphicCollector(Func<IPoint, double> expectedFunc, IPoint[] points)
        {
            _points = points;
            _expectedFunc = expectedFunc;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            var network = trainState.Problem.Network;
            var prevNetwork = trainState.Problem.PreviousNetwork;
            for (int i = 0; i < _points.Length; i++)
                yield return new Point(_points[i].Coordinate
                    .Concat(new[] { _expectedFunc(_points[i]) - network.Compute(_points[i].Coordinate) - (prevNetwork != null ? prevNetwork.Compute(_points[i].Coordinate) : 0.0) }).ToArray());
        }
    }
}
