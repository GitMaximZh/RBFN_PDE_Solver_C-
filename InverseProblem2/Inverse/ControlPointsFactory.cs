using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace InverseProblem2.Inverse
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double Alfa { get; set; }
        public double Beta { get; set; }

        public RBFNetwork.RBFNetwork DirectApproximator { get; set; }
        
        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetBoundPoints((CoefficientProblem)problem).Concat(GetInnerPoints((CoefficientProblem)problem));
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }


        protected virtual IControlPoint[] GetBoundPoints(CoefficientProblem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            for (int i = 0; i < XDimention; i++)
            {
                points.Add(new BoundControlPoint(problem, Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]) { Tag = 1 });
                points.Add(new BoundControlPoint(problem, Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i, TopRightCorner.Coordinate[1]) { Tag = 2 });
                points.Add(new BoundControlPoint1(problem, Beta, DirectApproximator, 1, ButtomLeftCorner.Coordinate[0] + xstep * i, ButtomLeftCorner.Coordinate[1]) { Tag = 5 });
                points.Add(new BoundControlPoint1(problem, Beta, DirectApproximator, 1, ButtomLeftCorner.Coordinate[0] + xstep * i, TopRightCorner.Coordinate[1]) { Tag = 6 });
            }

            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);
            for (int i = 1; i < YDimention - 1; i++)
            {
                points.Add(new BoundControlPoint(problem, Alfa, ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 3 });
                points.Add(new BoundControlPoint(problem, Alfa, TopRightCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 4 });
                points.Add(new BoundControlPoint1(problem, Beta, DirectApproximator, 0, ButtomLeftCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 7 });
                points.Add(new BoundControlPoint1(problem, Beta, DirectApproximator, 0, TopRightCorner.Coordinate[0], ButtomLeftCorner.Coordinate[1] + ystep * i) { Tag = 8 });
            }
            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(CoefficientProblem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 1; i < XDimention - 1; i++)
            {
                for (int j = 1; j < YDimention - 1; j++)
                {
                    points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            return points.ToArray();
        }
    }
}
