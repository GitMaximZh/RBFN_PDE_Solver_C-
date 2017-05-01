using System;

namespace Common
{
    public interface IPoint : ICloneable
    {
        double[] Coordinate { get; }
        int Dimension { get;  }
        double this[int d] { get; set; }
    }

    public class Point : IPoint
    {
        public double[] Coordinate { get; private set; }
        public int Dimension { get; private set; }

        public Point(int dimension = 1)
        {
            Dimension = dimension;
            Coordinate = new double[Dimension];
        }

        public Point(params double[] _coordinate)
        {
            if (_coordinate == null || _coordinate.Length == 0)
                throw new ArgumentException();

            Dimension = _coordinate.Length;
            Coordinate = new double[Dimension];

            for (int i = 0; i < Dimension; i++)
                Coordinate[i] = _coordinate[i];
        }

        public double this[int d]
        {
            get
            {
                if (d >= Dimension)
                    throw new ArgumentOutOfRangeException();
                return Coordinate[d];
            }
            set
            {
                if (d >= Dimension)
                    throw new ArgumentOutOfRangeException();
                Coordinate[d] = value;
            }
        }
        
        public virtual object Clone()
        {
            return new Point(Coordinate); 
        }
    }
}
