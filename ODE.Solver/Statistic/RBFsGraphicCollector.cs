using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using PDE.Statistic;
using RBFNetwork.Train;

namespace ODE.Solver.Statistic
{
    public class RBFsGraphicCollector : GraphicCollector
    {
        private readonly IPoint[] _points;

        public RBFsGraphicCollector(IPoint[] points)
        {
            _points = points;
        }

        protected override IEnumerable<Point> BuildGrapthic(Trainer.TrainStateArg trainState)
        {
            var network = trainState.Problem.Network;
            for (int i = 0; i < _points.Length; i++)
            {
                IList<double> values = new List<double>();
                for (int j = 0; j < network.HiddenCount; j++)
                {
                    values.Add(network.Weights[j] * network.Functions[j].Value(_points[i].Coordinate));
                }
                yield return new Point(_points[i].Coordinate
                                           .Concat(values).ToArray());
            }
        }
    }
}
