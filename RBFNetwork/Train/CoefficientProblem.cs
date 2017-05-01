using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using RBFNetwork.Common;

namespace RBFNetwork.Train
{
    public class CoefficientProblemParameters : ProblemParameters
    {
        public NetworkParameters ApproximatorParameters { get; set; }
    }

    public class CoefficientProblem : Problem
    {
        public RBFNetwork Approximator { get; protected set; }

        public CoefficientProblem(RBFNetwork network, RBFNetwork approximator)
            : base(network)
        {
            Approximator = approximator;
        }

        public void SplitPoint(Point point, ProblemParameters parametersToOptimize, out Point pN, out Point pA)
        {
            var pos = Network.GetParametersCount(parametersToOptimize.NetworkParameters);
            pN = new Point(point.Coordinate.Take(pos).ToArray());
            pA = new Point(point.Coordinate.Skip(pos).ToArray());
        }

            

        public override void MoveToPoint(Point point, ProblemParameters parametersToOptimize)
        {
            Point pN, pA;
            SplitPoint(point, parametersToOptimize, out pN, out pA);

            Network.MoveToPoint(pN, parametersToOptimize.NetworkParameters);
            Approximator.MoveToPoint(pA, ((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters);
        }

        public override void ClearHistory(ProblemParameters parametersToOptimize)
        {
            Network.ClearHistory(parametersToOptimize.NetworkParameters);
            Approximator.ClearHistory(((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters);
        }

        public override Point GetPoint(ProblemParameters parametersToOptimize)
        {
            return new Point(Network.GetPoint(parametersToOptimize.NetworkParameters).Coordinate
                .Concat(Approximator.GetPoint(((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters).Coordinate).ToArray());
        }

        public override void MoveBack(ProblemParameters parametersToOptimize)
        {
            Network.MoveBack(parametersToOptimize.NetworkParameters);
            Approximator.MoveBack(((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters);
        }

        public override IEnumerable<Parameter> GetOptimizedParameters(ProblemParameters parametersToOptimize)
        {
            return Network.GetParameters(parametersToOptimize.NetworkParameters)
                .Union(Approximator.GetParameters(((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters));
        }

        public override Parameter GetParameterByCoordinate(int coordinate, ProblemParameters parametersToOptimize)
        {
            var pos = Network.GetParametersCount(parametersToOptimize.NetworkParameters);
            if(coordinate < pos)
                return Network.GetParameterByCoordinate(coordinate, parametersToOptimize.NetworkParameters);
            return Approximator.GetParameterByCoordinate(coordinate - pos, ((CoefficientProblemParameters)parametersToOptimize).ApproximatorParameters);
        }
    }
}
