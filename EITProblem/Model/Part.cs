using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;

namespace EITProblem.Model
{
    public abstract class Part : IEnumerable<Point>
    {
        public Vector Normal { get; private set; }
        public double Weight { get; private set; }
        public double Value { get; protected set; }
        public Point From { get; private set; }
        public Point To { get; private set; }
        public int Density { get; private set; }
        public double Length { get; protected set; }

        protected List<Point> points;

        public Part(Vector normal, double weight, Point from, Point to, int density)
        {
            Normal = normal;
            Weight = weight;
            From = from;
            To = to;
            Density = density;
            Length = (new DenseVector(To.Coordinate) - new DenseVector(From.Coordinate)).Norm(2);

            points = new List<Point>();
            var vStep = CalculateStep();
            if (vStep.Norm(2) < 0.0001)
                points.Add(from);
            else
            {
                var vFrom = new DenseVector(From.Coordinate) + vStep;
                for (int i = 1; i < density; i++, vFrom += vStep)
                {
                    points.Add(new Point(vFrom.Values));
                }
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return points.GetEnumerator();
        }

        private DenseVector CalculateStep()
        {
            return (new DenseVector(To.Coordinate) - new DenseVector(From.Coordinate)) / Density;
        }
    }

    public class OpenPart : Part
    {
        public OpenPart(Vector normal, double weight, Point from, Point to, int density)
            : base(normal, weight, from, to, density)
        {
        }
    }

    public class Electrode : Part
    {
        public double CumValue { get; private set; }

        public Electrode(Vector normal, double weight, Point from, Point to, int density, double cumValue)
            : base(normal, weight, from, to, density)
        {
            CumValue = cumValue;
            Value = cumValue / Length;
            points.Insert(0, from);
            points.Add(to);
        }
    }
}
