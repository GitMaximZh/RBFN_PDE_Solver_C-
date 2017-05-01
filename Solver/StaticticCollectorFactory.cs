using System;
using System.Linq;
using Common;
using EITProblem.Direct;
using PDE.Solution;
using PDE.Statistic;
using RBFNetwork.Train;
using Solver.Statistic;

namespace Solver
{
    internal class StaticticCollectorFactory
    {
        private Case _case;
        public StaticticCollectorFactory(Case ourCase)
        {
            _case = ourCase;
        }

        public ActualGrapthicCollector CreatActualGrapthicCollector()
        {
            var result = new ActualGrapthicCollector(_case.Solution,
                                                     _case.GraphicPoints ?? _case.ControlPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "actual_graphic";
            g.DataFile = "actual_graphic";
            return result;
        }
        
        public ErrorHistoryGrapthicCollector CreateErrorHistoryGrapthicCollector()
        {
            var result = new ErrorHistoryGrapthicCollector();
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "error_history";
            g.DataFile = "error_history";
            return result;
        }

        public ErrorMapCollector CreateErrorMapCollector()
        {
            var points = _case.ControlPoints;
            if (_case.GraphicPoints!= null && _case.GraphicPoints.All(p => p is IControlPoint))
                points = _case.GraphicPoints;
            var result = new ErrorMapCollector((IControlPoint[])points);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "error_map";
            g.DataFile = "error_map";
            return result;
        }

        public IterationInfoCollector CreateIterationInfoCollector()
        {
            var result = new IterationInfoCollector();
            new ShowIterationInfo(result);
            return result;
        }

        public NetworkGrapthicCollector CreateNetworkGrapthicCollector(int cfg = 0)
        {
            var result = new NetworkGrapthicCollector(_case.GraphicPoints ?? _case.ControlPoints, cfg);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "network_graphic" + cfg;
            g.DataFile = "network_graphic" + cfg;
            return result;
        }

        public NetworkGrapthicCollector CreateContourNetworkGrapthicCollector(int cfg = 0)
        {
            var result = new NetworkGrapthicCollector(_case.GraphicPoints ?? _case.ControlPoints, cfg);
            var g = new ShowGraphic(result);
            //g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "Contour" + cfg;
            g.DataFile = "network_graphic" + cfg;
            return result;
        }

        public RBFGrapthicCollector CreateRBFGrapthicCollector()
        {
            var result = new RBFGrapthicCollector();
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "rbf_settings";
            g.DataFile = "rbf_settings";
            return result;
        }

        public RelativeErrorCollector CreateRelativeErrorCollector(IPoint[] checkPoints = null)
        {
            var result = new RelativeErrorCollector(_case.Solution, checkPoints);
            new ShowRelativeError(result);
            return result;
        }

        public RelativeErrorGraphicCollector CreateRelativeErrorGraphicCollector()
        {
            var result = new RelativeErrorGraphicCollector(_case.Solution,
                                                     _case.GraphicPoints ?? _case.ControlPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "relative_error";
            g.DataFile = "relative_error";
            return result;
        }

        public RelativeErrorHistoryGraphicCollector CreateRelativeErrorHistoryGraphicCollector
            (params RelativeErrorCollector[] collectors)
        {
            var result = new RelativeErrorHistoryGraphicCollector(collectors);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "relative_error_history";
            g.DataFile = "relative_error_history";
            return result;
        }

        public ActualCoefficientAndNetworkCoefficientGraphicCollector CreateActualCoefficientAndNetworkCoefficientGraphicCollector()
        {
            var result = new ActualCoefficientAndNetworkCoefficientGraphicCollector(
                 //for inv 2
                //p => 10 * p.Coordinate[0], Enumerable.Range(0, 101).Select(i => new Point(0.01 * i)).ToArray());

                //for inv 3
                p => 0.5 + p.Coordinate[0], Enumerable.Range(0, 101).Select(i => new Point(0.01 * i)).ToArray());
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "coefficient_and_approximator_graphic";
            g.DataFile = "coefficient_and_approximator_graphic";
            return result;
        }

        public CoefficientRelativeErrorCollector CreateCoefficientRelativeErrorCollector(Func<IPoint, double> expected)
        {
            var result = new CoefficientRelativeErrorCollector(expected, _case.CheckPoints);
            new ShowCoefficientRelativeError(result);
            return result;
        }

        public ActualSliceAndNetworkSliceGraphicCollector CreateActualSliceAndNetworkSliceGraphicCollector(Func<IPoint, double> function, IPoint[] points, string p="")
        {
            var result = new ActualSliceAndNetworkSliceGraphicCollector(function, points);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "2d_config";
            g.GrapthicScript = "slice_graphic";
            g.DataFile = "slice_graphic" + p;
            return result;
        }

        public FunctionGraphicCollector CreateCoefficent3DGraphicCollector(Func<IPoint, double> function)
        {
            var result = new FunctionGraphicCollector(function, _case.GraphicPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "coefficient_3d";
            g.DataFile = "coefficient_3d";
            return result;
        }

        public FunctionGraphicCollector CreateCoefficent3DRelativeGraphicCollector(Func<IPoint, double> function)
        {
            var result = new FunctionGraphicCollector(function, _case.GraphicPoints);
            var g = new ShowGraphic(result);
            g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "coefficient_3d_relative";
            g.DataFile = "coefficient_3d_relative";
            return result;
        }

        public SliceRelativeErrorCollector CreateSliceRelativeErrorCollector(Func<IPoint, double> function, IPoint[] points)
        {
            var result = new SliceRelativeErrorCollector(function, points);
            new ShowSliceRelativeError(result);
            return result;
        }

        public ControlPointsErrorCollector CreateControlPointsErrorCollector(int tag)
        {
            var result = new ControlPointsErrorCollector(_case.ControlPoints.Cast<IControlPoint>().Where(p => p.Tag == tag));
            new ShowControlPointsError(result);
            return result;
        }

        public FunctionGraphicCollector CreateMatlabGraphicCollector(Func<IPoint, double> function, IPoint[] points)
        {
            var result = new FunctionGraphicCollector(function, _case.GraphicPoints);
            var g = new ExportGrapthic(result);
            g.DataFile = "Data";
            return result;
        }
    }
}
