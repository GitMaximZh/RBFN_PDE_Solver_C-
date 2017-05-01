using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace EITProblem
{
    public static class Utils
    {
        public static IPoint[] GetGraphicPoints(Point buttomLeftCorner, Point topRightCorner, int xDimention, int yDimention)
        {
            var points = new List<IPoint>();

            var xstep = (topRightCorner.Coordinate[0]
                          - buttomLeftCorner.Coordinate[0]) / (xDimention - 1);
            var ystep = (topRightCorner.Coordinate[1]
                          - buttomLeftCorner.Coordinate[1]) / (yDimention - 1);

            for (int i = 0; i <= xDimention - 1; i++)
            {
                for (int j = 0; j <= yDimention - 1; j++)
                {
                    points.Add(new Point(buttomLeftCorner.Coordinate[0] + xstep * i,
                                         buttomLeftCorner.Coordinate[1] + ystep * j));
                }
            }

            return points.ToArray();
        }
    }
}
