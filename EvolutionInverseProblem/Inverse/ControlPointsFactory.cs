using System;
using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace EvolutionInverseProblem.Inverse
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        private static readonly Random Rand = new Random(15123);

        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double Alfa { get; set; }
       
        public RBFNetwork.RBFNetwork DirectApproximator { get; set; }

        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetBoundPoints(problem).Concat(GetInnerPoints(problem));
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }
        
        protected virtual IControlPoint[] GetBoundPoints(Problem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (cX * XDimention - 1);
            for (int i = 0; i < cX* XDimention; i++)
            {
                points.Add(new BoundControlPoint(problem, 100, DirectApproximator.Value(new [] { ButtomLeftCorner.Coordinate[0] + xstep * i, 
                    TopRightCorner.Coordinate[1] }) + 2 * 0.01 * (Rand.NextDouble() - 0.5),
                    ButtomLeftCorner.Coordinate[0] + xstep * i, TopRightCorner.Coordinate[1]) { Tag = 1 });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (cY *YDimention - 1);
            for (int i = 0; i < cY*YDimention - 1; i++)
            {
                points.Add(new BoundControlPoint(problem, Alfa, 0, ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 2 });
                points.Add(new BoundControlPoint(problem, Alfa, 0, TopRightCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 3 });
            }
            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(Problem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 1; i < XDimention - 1; i++)
            {
                for (int j = 1; j < YDimention - 1; j++)
                {
                    points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            for (int i = 1; i < cX*XDimention - 1; i++)
            {
                points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + (xstep / cX) * i,
                                         ButtomLeftCorner.Coordinate[1]) { Tag = 4 });
            }

            return points.ToArray();
        }

        public int cY { get; set; }

        public int cX { get; set; }
    }
}
