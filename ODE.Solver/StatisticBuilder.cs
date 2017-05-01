using System.Collections.Generic;
using PDE.Solution;
using PDE.Statistic;

namespace ODE.Solver
{
    public static class StatisticBuilder
    {
        public static void Build(PDE.Statistic.Statistic statistic, Case ourCase)
        {
            var factory = new StaticticCollectorFactory(ourCase);
            statistic.AddStatisticCollector("Iteration", factory.CreateIterationInfoCollector());
            statistic.AddStatisticCollector("Iteration", factory.CreateRelativeErrorCollector());
            
            statistic.AddStatisticCollector("Train", factory.CreatActualAndNetworkGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreatCoefficientAndApproximatorGraphicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateRBFsGrapthicCollector());
        }
    }
}
