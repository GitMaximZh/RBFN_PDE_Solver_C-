using System;
using System.Linq;
using Common;
using RBFNetwork.Train;
namespace PoissonPDE
{
    internal class ControlPointsTransformer : IControlPointsTransformer
    {
        private static readonly Random Rand = new Random(5123);
        
        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }
        public int XSteps { get; set; }
        public int YSteps { get; set; }

        public int TransformEachXIteration { get; set; }

        private bool _shouldBeTransformed = false;

        public ControlPointsTransformer()
        {
            TransformEachXIteration = 10;
        }

        public bool ShouldBeTransformed(int step)
        {
            if (step % TransformEachXIteration == 0)
            {
                Console.WriteLine("Should control points be transforemed?");
                if (Console.ReadLine() == "1")
                    return true;
            }
            return false;
        }

        public IControlPoint[] Transform(IControlPoint[] points)
        {
            foreach (var controlPoint in points.Where(p => p.Tag == 0))
            {
                controlPoint.Coordinate[0] = Move(controlPoint.Coordinate[0], ButtomLeftCorner[0] + 0.001,
                                                  TopRightCorner[0] - 0.001, XSteps);
                controlPoint.Coordinate[1] = Move(controlPoint.Coordinate[1], ButtomLeftCorner[1] + 0.001,
                                                  TopRightCorner[1] - 0.001, YSteps);
            }

            foreach (var controlPoint in points.Where(p => p.Tag == 1 || p.Tag == 2))
            {
                controlPoint.Coordinate[0] = Move(controlPoint.Coordinate[0], ButtomLeftCorner[0],
                                                  TopRightCorner[0], XSteps);
            }

            foreach (var controlPoint in points.Where(p => p.Tag == 3 || p.Tag == 4))
            {
                controlPoint.Coordinate[1] = Move(controlPoint.Coordinate[1], ButtomLeftCorner[1],
                                                  TopRightCorner[1], YSteps);
            }
            return points;
        }

        private double Move(double current, double b1, double b2, int steps)
        {
            var movement = (Rand.NextDouble() - 0.5) * (b2 - b1) / steps;
            if (current + movement > b2 || current + movement < b1)
                return current - movement;
            return current + movement;
        }
    }
}
