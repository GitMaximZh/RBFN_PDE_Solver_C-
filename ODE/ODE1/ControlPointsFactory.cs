using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace ODE.ODE1
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }

        public Point Left { get; set; }
        public Point Right { get; set; }

        public double Alfa { get; set; }

        public ControlPoint[] Create()
        {
            int index = 1;
            var points = GetBoundPoints().Concat(GetInnerPoints());
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }


        protected virtual ControlPoint[] GetBoundPoints()
        {
            var points = new List<ControlPoint>();
            points.Add(new BoundControlPoint(Alfa, 0) { Tag = 1 });
            points.Add(new DxBoundControlPoint(Alfa, 0) { Tag = 2 });
            
            return points.ToArray();
        }

        protected virtual ControlPoint[] GetInnerPoints()
        {
            var points = new List<ControlPoint>();

            var xstep = (Right.Coordinate[0] - Left.Coordinate[0]) / (XDimention - 1);

            for (int i = 1; i < XDimention; i++)
            {
                points.Add(new InnerControlPoint(1, Left.Coordinate[0] + xstep * i) { Tag = 0 });
            }

            return points.ToArray();
        }
    }
}
