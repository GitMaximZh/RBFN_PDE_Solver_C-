using System.Collections.Generic;
using System.Linq;
using Common;
using PDE.Solution;
using RBFNetwork.Train;

namespace WavePDE
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double C1Alfa { get; set; }
        public double C2Alfa { get; set; }
        public double C3Alfa { get; set; }
        public double C4Alfa { get; set; }

        public ControlPoint[] Create()
        {
            int index = 1;
            var points = GetBoundPoints(ref index).Concat(GetInnerPoints(ref index));
            return points.ToArray();
        }


        protected virtual ControlPoint[] GetBoundPoints(ref int index)
        {
            var points = new List<ControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - (ButtomLeftCorner.Coordinate[0])) / (XDimention - 1);
            for (int i = 0; i < XDimention; i++)
            {
                var point = new Point(ButtomLeftCorner.Coordinate[0] + xstep*i, ButtomLeftCorner.Coordinate[1]);
                points.Add(new Fx0BoundControlPoint(C1Alfa, point.Coordinate) { Index = index });
                points.Add(new DtFx0BoundControlPoint(C2Alfa, point.Coordinate) { Index = index++ });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - (ButtomLeftCorner.Coordinate[1])) / (YDimention - 1);
            for (int i = 0; i < YDimention; i++)
            {
                var point = new Point(ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i);
                points.Add(new F0tBoundControlPoint(C3Alfa, point.Coordinate) { Index = index });
                points.Add(new DxF0tBoundControlPoint(C4Alfa, point.Coordinate) { Index = index++ });
            }

            return points.ToArray();
        }

        protected virtual ControlPoint[] GetInnerPoints(ref int index)
        {
            var points = new List<ControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - (ButtomLeftCorner.Coordinate[0])) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 1; i < XDimention; i++)
            {
                for (int j = 1; j < YDimention; j++)
                {
                    points.Add(new InnerControlPoint(1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Index = index++ });
                }
            }

            return points.ToArray();
        }
    }
}
