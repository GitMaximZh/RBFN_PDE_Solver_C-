using System.Collections.Generic;
using Common;
using NonlinerPoissonPDE;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace NonlinerPoissonPDE
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int Cf { get; set; }
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

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

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention * Cf - 1);
            for (int i = 0; i < XDimention * Cf; i++)
            {
                points.Add(new BoundControlPoint(Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]) { Tag = 1 });
                points.Add(new BoundControlPoint(Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i, TopRightCorner.Coordinate[1]) { Tag = 2 });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention * Cf - 1);
            for (int i = 0; i < YDimention * Cf; i++)
            {
                points.Add(new DxF0yBoundControlPoint(Alfa, ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 3 });
                points.Add(new BoundControlPoint(Alfa, TopRightCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 4 });
            }
            return points.ToArray();
        }

        protected virtual ControlPoint[] GetInnerPoints()
        {
            var points = new List<ControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 0; i < XDimention; i++)
            {
                for (int j = 0; j < YDimention; j++)
                {
                    points.Add(new InnerControlPoint(1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            return points.ToArray();
        }
    }
}
