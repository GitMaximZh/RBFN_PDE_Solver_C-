using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class ErrorMapCollector : GraphicCollector
    {
        private readonly IControlPoint[] _points;

        public ErrorMapCollector()
            : this(null)
        {
            
        }

        public ErrorMapCollector(IControlPoint[] points)
        {
            _points = points;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            var points = _points ?? trainState.ControlPoints;
            foreach (var gr in points.GroupBy(cp => cp.Index))
            {
                var error = 0.0;
                foreach (var p in gr)
                {
                    error += p.Weight * Math.Pow(p.ApproximateValue()
                                                   - p.ExpectedValue(), 2);
                }
                var point = gr.First();
                yield return new Point(point.Coordinate
                    .Concat(new[] { error }).ToArray());
            }
        }
    }
}
