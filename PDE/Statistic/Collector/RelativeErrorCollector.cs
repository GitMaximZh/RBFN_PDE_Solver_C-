using System;
using System.Data;
using System.Linq;
using Common;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class RelativeErrorCollector : StatisticCollector
    {
        private readonly IPoint[] _controlPoints;
        private readonly Func<IPoint, double> _expectedFunc;

        public double RelativeError { get; private set; }
    
        public RelativeErrorCollector(Func<IPoint, double> expectedFunc)
            : this(expectedFunc, null)
        {
        }

        public RelativeErrorCollector(Func<IPoint, double> expectedFunc, IPoint[] points)
        {
            _controlPoints = points;
            _expectedFunc = expectedFunc;
        }


        public override void Collect(Trainer.TrainStateArg iteration)
        {
            var network = iteration.Problem.Network;
            var prevNetwork = iteration.Problem.PreviousNetwork;
            var points = _controlPoints ?? iteration.ControlPoints;

            var error = 0.0;
            var exact = 0.0;
            var error1 = 0.0; 

            foreach (var point in points)
            {
                double pReal = network.Compute(point.Coordinate) + (prevNetwork != null ? prevNetwork.Compute(point.Coordinate) : 0.0);
                double pExact = _expectedFunc(point);
                //if (Math.Abs(pReal - pExact) > error)
                //    error = Math.Abs(pReal - pExact);
                error += Math.Pow(pReal - pExact, 2);
                exact += Math.Pow(pExact, 2);
                error1 += Math.Abs(pReal - pExact);
            }

            //foreach (var t in new[] { 1, 3, 5, 7, 10 })
            //{
            //    var e = 0.0;
            //    for (double x = -1; x <= 1.001; x += 0.02)
            //    {

            //        var p = new Point(x, t);
            //        double pReal = network.Compute(p.Coordinate);
            //        double pExact = _expectedFunc(p);
            //        //if (Math.Abs(pReal - pExact) > error)
            //        //e = Math.Max(Math.Abs(pReal - pExact), e);
            //        e += Math.Abs(pReal - pExact);
            //        //exact += Math.Pow(pExact, 2);
            //    }
            //    //Console.Write("T: {0} - {1}  ", t, Math.Sqrt(e / 100));
            //}

            //Console.Write("T: {0}", error1);
            RelativeError = Math.Sqrt(error) / Math.Sqrt(exact);
        }
    }
}
