using System.Collections.Generic;
using EITProblem.Direct;
using PDE.Solution;
using PDE.Statistic;
using RBFNetwork.Train;
using Solver.Statistic;

namespace Solver
{
    public static class StatisticBuilder
    {
        public static void Build(PDE.Statistic.Statistic statistic, Case ourCase)
        {
            var factory = new StaticticCollectorFactory(ourCase);
            statistic.AddStatisticCollector("Iteration", factory.CreateIterationInfoCollector());

            var rCs = new List<RelativeErrorCollector>();

            var r1 = factory.CreateRelativeErrorCollector(ourCase.CheckPoints);
            statistic.AddStatisticCollector("Iteration", r1);
            //rCs.Add(r1);

            //statistic.AddStatisticCollector("Iteration", factory.CreateControlPointsErrorCollector(4));
            //if (ourCase.CheckPoints != null)
            //{
            //    var r2 = factory.CreateRelativeErrorCollector(ourCase.CheckPoints);
            //    statistic.AddStatisticCollector("Iteration", r2);
            //    rCs.Add(r2);
            //}
            //statistic.AddStatisticCollector("IterationCollect&TrainShow", factory.CreateRelativeErrorHistoryGraphicCollector(rCs.ToArray()));

            statistic.AddStatisticCollector("Train", factory.CreatActualGrapthicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateNetworkGrapthicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateContourNetworkGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateNetworkGrapthicCollector(1));
            //statistic.AddStatisticCollector("Train", factory.CreateNetworkGrapthicCollector(2));
            //statistic.AddStatisticCollector("Train", factory.CreateErrorHistoryGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateRBFGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateErrorMapCollector());



            //statistic.AddStatisticCollector("Train", factory.CreateRelativeErrorGraphicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateCoefficientRelativeErrorCollector());
        }

        public static void BuildEIT(PDE.Statistic.Statistic statistic, Case ourCase)
        {
            var factory = new StaticticCollectorFactory(ourCase);
            
            statistic.AddStatisticCollector("Iteration", factory.CreateIterationInfoCollector());
            statistic.AddStatisticCollector("Iteration", factory.CreateRelativeErrorCollector(ourCase.CheckPoints));
            statistic.AddStatisticCollector("Iteration", factory.CreateControlPointsErrorCollector(5));
            statistic.AddStatisticCollector("Iteration", factory.CreateCoefficientRelativeErrorCollector(p => DirectSuite.Resistency(p.Coordinate)));

            statistic.AddStatisticCollector("Train", factory.CreateNetworkGrapthicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateRelativeErrorGraphicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateCoefficent3DGraphicCollector(p => ((CoefficientProblem)ourCase.Problem).Approximator.Compute(p.Coordinate)));
            statistic.AddStatisticCollector("Train", factory.CreateCoefficent3DRelativeGraphicCollector
                (p => DirectSuite.Resistency(p.Coordinate) - ((CoefficientProblem)ourCase.Problem).Approximator.Compute(p.Coordinate)));

            //statistic.AddStatisticCollector("Train", factory.CreateRBFGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateErrorMapCollector());




        }

        public static void BuildForReport(PDE.Statistic.Statistic statistic, Case ourCase)
        {
            var factory = new StaticticCollectorFactory(ourCase);

            statistic.AddStatisticCollector("Iteration", factory.CreateIterationInfoCollector());
            statistic.AddStatisticCollector("Iteration", factory.CreateRelativeErrorCollector(ourCase.CheckPoints));

            statistic.AddStatisticCollector("Train", factory.CreateNetworkGrapthicCollector());
            statistic.AddStatisticCollector("Train", factory.CreateMatlabGraphicCollector(p => ourCase.Problem.Network.Compute(p.Coordinate), ourCase.GraphicPoints));
            statistic.AddStatisticCollector("Train", factory.CreatActualGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateRBFGrapthicCollector());
            //statistic.AddStatisticCollector("Train", factory.CreateErrorMapCollector());
        }
    }
}
