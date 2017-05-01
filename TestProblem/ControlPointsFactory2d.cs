using System.Collections.Generic;
using System.Linq;
using Common;
using PDE.Solution;
using RBFNetwork.Train;

namespace TestProblem
{
    internal class ControlPointsFactory2d : IControlPointsFactory
    {
        public int TDimention { get; set; }

        public int XDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double Alfa { get; set; }

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
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            for (int i = 0; i < XDimention; i++)
                for (int j = 0; j < XDimention; j++)
                    points.Add(new InitialIntensityControlPoint(problem, Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]) { Tag = 1 });

            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(Problem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var tstep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (TDimention - 1);

            for (int i = 0; i < XDimention; i++)
            {
                for (int k = 1; k < TDimention; k++)
                {
                    points.Add(new InnerControlPoint2d(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1] + tstep * k) { Tag = 0 });
                }
            }

            return points.ToArray();
        }
    }
}
