using System.Collections.Generic;
using System.Linq;
using Common;

namespace Plotting
{
    public class Function
    {
        private class PointComparer : IComparer<Point>
        {

            public int Compare(Point x, Point y)
            {
                if (x.Dimension != y.Dimension || x.Dimension == 1)
                    return 0;
                for (int i = 0; i < x.Dimension - 1; i++)
                    if (x[i] != y[i])
                        return x[i] > y[i] ? 1 : -1;
                return 0;
            }
        }

        private SortedSet<Point> points = new SortedSet<Point>(new  PointComparer());
        private Point[] aPoints = null;

        public Point[] Points 
        { 
            get 
            { 
                if(aPoints == null) 
                    aPoints = points.ToArray();
                return aPoints;
            } 
        }

        public Function(params Point[] _points)
        {
            if (_points != null)
            {
                foreach (Point p in _points)
                    points.Add(p);
            }            
        }

        public bool AddPoint(Point point)
        {
            aPoints = null;
            return points.Add(point);
        }
    }
}
