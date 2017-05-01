using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace EvolutionInverseProblem.Direct
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

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
                          - ButtomLeftCorner.Coordinate[0]) / (cX * XDimention - 1);
            for (int i = 0; i < cX * XDimention; i++)
            {
                var x = ButtomLeftCorner.Coordinate[0] + xstep*i;
                double u0x = -2.0 * (x - 0.5) * (x - 0.5) + 0.5;
                //if (x <= 0.3)
                //    u0x = x / 0.3;
                //else
                //    u0x = (1.0 - x)/0.7;
                points.Add(new BoundControlPoint(problem, Alfa, u0x, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]) { Tag = 1 });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (cY * YDimention - 1);
            for (int i = 1; i < cY * YDimention; i++)
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
                for (int j = 1; j < YDimention; j++)
                {
                    points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            return points.ToArray();
        }

        public int cY { get; set; }

        public int cX { get; set; }
    }
}
