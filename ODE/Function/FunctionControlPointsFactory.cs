using System;
using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace ODE.Function
{
    internal class FunctionControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        
        public Point Left { get; set; }
        public Point Right { get; set; }

        public Func<double, double> Function { get; set; }

        public ControlPoint[] Create()
        {
            int index = 1;
            var points = GetControlPoints();
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }

        protected virtual ControlPoint[] GetControlPoints()
        {
            var points = new List<ControlPoint>();

            var xstep = (Right.Coordinate[0] - Left.Coordinate[0]) / (XDimention - 1);

            for (int i = 1; i < XDimention - 1; i++)
            {
                points.Add(new FunctionControlPoint(Function, 1, Left.Coordinate[0] + xstep * i) { Tag = 0 });
            }

            return points.ToArray();
        }
    }
}
