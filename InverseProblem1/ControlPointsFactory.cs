using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace InverseProblem1
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        public double Alfa { get; set; }
        
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
                         - ButtomLeftCorner.Coordinate[0]) / (10 - 1);

            for (int i = 0; i < 10; i++)
            {
                points.Add(new BoundControlPoint(problem, Alfa, ButtomLeftCorner.Coordinate[0] + xstep * i) { Tag = 1 });
            }

            //points.Add(new BoundControlPoint(problem, Alfa, 1.0) { Tag = 1 });
            //points.Add(new BoundControlPoint(problem, Alfa, 2.0) { Tag = 2 });

            points.Add(new BoundControlPoint1(problem, Alfa, 1.0) { Tag = 2 });
            
            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(CoefficientProblem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);

            for (int i = 0; i < XDimention; i++)
            {
                points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep*i) {Tag = 0});
            }

            return points.ToArray();
        }
    }
}
