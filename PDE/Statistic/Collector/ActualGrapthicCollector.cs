using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class ActualGrapthicCollector : GraphicCollector
    {
        private readonly IPoint[] _points;
        private readonly Func<IPoint, double> _function;

        public ActualGrapthicCollector(Func<IPoint, double> function, IPoint[] points)
        {
            _points = points;
            _function = function;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            for (int i = 0; i < _points.Length; i++)
                yield return new Point(_points[i].Coordinate
                    .Concat(new [] { _function(_points[i]) }).ToArray());
        }
    }
}
