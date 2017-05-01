using System.Collections.Generic;
using Common;
using RBFNetwork.Common;

namespace RBFNetwork.Train
{
    public class ProblemParameters
    {
        public NetworkParameters NetworkParameters { get; set; }
    }

    public class Problem
    {
        public RBFNetwork Network { get; protected set; }

        public RBFNetwork PreviousNetwork { get; set; }

        public Problem(RBFNetwork network)
        {
            Network = network;
        }

        public virtual void MoveToPoint(Point point, ProblemParameters parametersToOptimize)
        {
           Network.MoveToPoint(point, parametersToOptimize.NetworkParameters);
        }

        public virtual void ClearHistory(ProblemParameters parametersToOptimize)
        {
            Network.ClearHistory(parametersToOptimize.NetworkParameters);
        }

        public virtual Point GetPoint(ProblemParameters parametersToOptimize)
        {
            return Network.GetPoint(parametersToOptimize.NetworkParameters);
        }

        public virtual void MoveBack(ProblemParameters parametersToOptimize)
        {
            Network.MoveBack(parametersToOptimize.NetworkParameters);
        }

        public virtual IEnumerable<Parameter> GetOptimizedParameters(ProblemParameters parametersToOptimize)
        {
            return Network.GetParameters(parametersToOptimize.NetworkParameters);
        }

        public virtual Parameter GetParameterByCoordinate(int coordinate, ProblemParameters parametersToOptimize)
        {
            return Network.GetParameterByCoordinate(coordinate, parametersToOptimize.NetworkParameters);
        }
    }
}

    
