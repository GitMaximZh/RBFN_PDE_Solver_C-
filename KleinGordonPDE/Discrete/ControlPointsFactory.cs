using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using PDE.Solution;
using RBFNetwork.Train;

namespace KleinGordonPDE.Discrete
{
    public class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }

        public Point LeftCorner { get; set; }
        public Point RightCorner { get; set; }

        public double C1Alfa { get; set; }
        public double C2Alfa { get; set; }

        public Problem UN { get; set; }
        public Problem UN_1 { get; set; }

        public double T { get; set; }
        public double dT { get; set; }

        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetBoundPoints(problem, ref index).Concat(GetInnerPoints(problem, ref index));
            return points.ToArray();
        }


        protected virtual IControlPoint[] GetBoundPoints(Problem problem, ref int index)
        {
            var points = new List<IControlPoint>();
            points.Add(new F0tBoundControlPoint(problem, T, C1Alfa, LeftCorner.Coordinate) { Index = index++ });
            points.Add(new F1tBoundControlPoint(problem, T, C2Alfa, RightCorner.Coordinate) { Index = index++ });

            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(Problem problem, ref int index)
        {
            var points = new List<IControlPoint>();

            var xstep = (RightCorner.Coordinate[0]
                          - LeftCorner.Coordinate[0]) / (XDimention - 1);

            for (int i = 1; i < XDimention - 1; i++)
            {
                points.Add(new InnerControlPoint(problem, UN, UN_1, T, dT, 1, LeftCorner.Coordinate[0] + xstep * i) { Index = index++ });
            }

            return points.ToArray();
        }
    }
}

