using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using KleinGordonPDE;
using PDE.Solution;
using RBFNetwork.Train;

namespace Iteration.Solver
{
    public class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double C1Alfa { get; set; }
        public double C2Alfa { get; set; }
        public double C3Alfa { get; set; }
        public double C4Alfa { get; set; }

        public Problem PreviousProblem { get; set; }
        
        public int Cf { get; set; }
        
        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetBoundPoints(problem, ref index).Concat(GetInnerPoints(problem, ref index));
            return points.ToArray();
        }


        protected virtual IControlPoint[] GetBoundPoints(Problem problem, ref int index)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - (ButtomLeftCorner.Coordinate[0])) / (XDimention * Cf - 1);
            for (int i = 1; i < XDimention * Cf - 1; i++)
            {
                var point = new Point(ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]);
                points.Add(new Fx0BoundControlPoint(problem, C1Alfa, PreviousProblem == null ? point[0] : PreviousProblem.Network.Compute(point.Coordinate),
                    point.Coordinate) { Index = index });
                points.Add(new DtFx0BoundControlPoint(problem, C2Alfa, PreviousProblem == null ? 0 : PreviousProblem.Network.FirstDerivation(point.Coordinate, 1), point.Coordinate) { Index = index++ });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - (ButtomLeftCorner.Coordinate[1])) / (YDimention * Cf - 1);
            for (int i = 0; i < YDimention * Cf; i++)
            {
                var point1 = new Point(ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i);
                var point2 = new Point(TopRightCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i);
                points.Add(new F0tBoundControlPoint(problem, C3Alfa, point1.Coordinate) { Index = index++ });
                points.Add(new F1tBoundControlPoint(problem, C4Alfa, point2.Coordinate) { Index = index++ });
            }

            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(Problem problem, ref int index)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - (ButtomLeftCorner.Coordinate[0])) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 1; i < XDimention -1; i++)
            {
                for (int j = 1; j < YDimention; j++)
                {
                    points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Index = index++ });
                }
            }

            return points.ToArray();
        }
    }
}

