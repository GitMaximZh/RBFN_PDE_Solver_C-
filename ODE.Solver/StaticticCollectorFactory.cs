using System.Linq;
using Common;
using ODE.Solver.Statistic;
using PDE.Solution;
using PDE.Statistic;
using Solver.Statistic;

namespace ODE.Solver
{
    internal class StaticticCollectorFactory
    {
        private Case _case;
        public StaticticCollectorFactory(Case ourCase)
        {
            _case = ourCase;
        }

        public ActualAndNetworkGraphicCollector CreatActualAndNetworkGrapthicCollector()
        {
            var result = new ActualAndNetworkGraphicCollector(_case.Solution,
                                                     _case.GraphicPoints ?? _case.ControlPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "actual_and_network_graphic";
            g.DataFile = "actual_and_network_graphic";
            return result;
        }

        public CoefficientAndApproximatorGraphicCollector CreatCoefficientAndApproximatorGraphicCollector()
        {
            var result = new CoefficientAndApproximatorGraphicCollector(p => 1.0 / p[0],
                                                     _case.GraphicPoints ?? _case.ControlPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "coefficient_and_approximator_graphic";
            g.DataFile = "coefficient_and_approximator_graphic";
            return result;
        }

        public RBFsGraphicCollector CreateRBFsGrapthicCollector()
        {
            var result = new RBFsGraphicCollector(_case.GraphicPoints ?? _case.ControlPoints);
            var g = new ShowRBFsGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "rbfs_history";
            g.DataFile = "rbfs_history";
            return result;
        }

        public IterationInfoCollector CreateIterationInfoCollector()
        {
            var result = new IterationInfoCollector();
            new ShowIterationInfo(result);
            return result;
        }

        public RelativeErrorCollector CreateRelativeErrorCollector(IPoint[] checkPoints = null)
        {
            var result = new RelativeErrorCollector(_case.Solution, checkPoints);
            new ShowRelativeError(result);
            return result;
        }
    }
}
