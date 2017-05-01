using System;
using System.Collections.Generic;
using Common;
using EITProblem.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace EITProblem.Coefficient
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetPoints(problem);
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }
        
        protected virtual IControlPoint[] GetPoints(Problem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 0; i <= XDimention - 1; i++)
            {
                for (int j = 0; j <= YDimention - 1; j++)
                {
                    points.Add(new CoefficientControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            return points.ToArray();
        }
    }
}
