using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using PDE.Statistic;
using RBFNetwork.Train;

namespace Solver.Statistic
{
    public class ActualSliceAndNetworkSliceGraphicCollector : GraphicCollector
    {
        private readonly IPoint[] _points;
        private readonly Func<IPoint, double> _function;

        public ActualSliceAndNetworkSliceGraphicCollector(Func<IPoint, double> function, IPoint[] points)
        {
            _points = points;
            _function = function;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            for (int i = 0; i < _points.Length; i++)
                yield return new Point(_points[i].Coordinate[0], _function(_points[i]), _points[i].Coordinate[0], trainState.Problem.Network.Compute(_points[i].Coordinate));
        }
    }
}
