using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;

namespace OptimizationToolbox.Common
{
    public static class PointExtention
    {
        public static Point Plus(this Point point, DenseVector vector)
        {
            if (point.Dimension != vector.Count)
                throw new ArgumentException();
            Point newPoint = new Point(point.Dimension);
            for (int i = 0; i < newPoint.Dimension; i++)
                newPoint[i] = point[i] + vector[i];
            return newPoint;
        }
    }
}
