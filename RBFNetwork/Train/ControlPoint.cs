using System;
using Common;
using RBFNetwork.Function;

namespace RBFNetwork.Train
{
    public interface IControlPoint : IPoint
    {
        int CurrentPassId { get; set; }

        int Index { get; set; }
        int Tag { get; set; }
        double Weight { get; }
        double ApproximateValue(ITwiceDifferentiableFunction func = null);
        double ExpectedValue();
    }

    public abstract class ControlPoint<T> : Point, IControlPoint
        where T : Problem
    {
        protected T Problem { get; private set; }
        public double Weight { get; private set; }
        public int Tag { get; set; }
        public int Index { get; set; }

        public int CurrentPassId { get; set; }

        public ControlPoint(T problem, double weight, Point point)
            : this(problem, weight, point.Coordinate)
        {
        }

        public ControlPoint(T problem, double weight, params double[] coordinate)
            : base(coordinate)
        {
            Problem = problem;
            Weight = weight;
        }

        public ControlPoint(T problem, double weight, int dimention = 1)
            : base(dimention)
        {
            Problem = problem;
            Weight = weight;
        }

        public abstract double ApproximateValue(ITwiceDifferentiableFunction func = null);
        public abstract double ExpectedValue();
    }
}
