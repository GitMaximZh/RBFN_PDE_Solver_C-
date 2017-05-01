using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using PDE;
using PDE.Optimizer;
using PDE.Solution;
using PoissonPDE.Gradient;
using PoissonPDE.QuasiNewton;
using PoissonPDE.TrustRegion;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace PoissonPDE
{
    public class MethodsCompareSuite
    {
        /*
        private static readonly Random Rand = new Random(15123);

        private static readonly RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2)
            };

        private static readonly ControlPoint[] GraphicPoints =  new ControlPointsFactory()
            {
                ButtomLeftCorner = new Point(0, 0),
                TopRightCorner = new Point(1, 1),
                XDimention = 12,
                YDimention = 12
            }.Create();

        private static readonly Point[] CheckPoints =  
             GenerateRandomCheckPoints(100, 11, new Point(0, 0), new Point(1, 1));

        private static readonly Func<Point, double> Solution =
            p => -1.0*Math.Sin(Math.PI*p.Coordinate[0])*Math.Sin(Math.PI*p.Coordinate[1])/(2*Math.Pow(Math.PI, 2));

        #region LSM

        public static Case Case_LSM_1(double width = 0.3)
        {
            var optimizer = new SVDOptimizer();

            var settings = DefaultSettings;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0;
            settings.Width = width;
            settings.WidthDeviation = 0;
            settings.StartPoint = new Point(0, 0);
            settings.EndPoint = new Point(1, 1);
            settings.CentersXDimention = 6;
            settings.CentersYDimention = 6;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 11,
                    YDimention = 11
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = GraphicPoints;
            ourCase.Solution = Solution;
            ourCase.CheckPoints = CheckPoints;
            return ourCase;
        }

        #endregion

        #region TR

        public static Case Case_TR_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 16;
            settings.Width = 0.45;
            settings.WidthDeviation = 0.15;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0.05;
            settings.StartPoint = new Point(-0.2, -0.2);
            settings.EndPoint = new Point(1.2, 1.2);

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = GraphicPoints;
            ourCase.Solution = Solution;
            ourCase.CheckPoints = CheckPoints;
            return ourCase;
        }

        #endregion

        #region LM

        public static Case Case_LM_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new LevenbergMarquardtMethodFactory { MinStep = 0.000001, Tau = 0.1 });
            var settings = DefaultSettings;
            settings.FunctionsCount = 16;
            settings.Weight = 0;
            settings.WeightDeviation = 0.5;
            settings.Width = 0.5;
            settings.WidthDeviation = 0.05;
            settings.StartPoint = new Point(-0.2, -0.2);
            settings.EndPoint = new Point(1.2, 1.2);

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    Alfa = 2000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = GraphicPoints;
            ourCase.Solution = Solution;
            ourCase.CheckPoints = CheckPoints;
            return ourCase;
        }

        #endregion 
        
        private static Point[] GenerateRandomCheckPoints(int innerPointsCount, int eachBoundPointsCount, Point buttomLeft, Point topRight)
        {
            var width = topRight.Coordinate[0] - buttomLeft.Coordinate[0] - 0.02;
            var height = topRight.Coordinate[1] - buttomLeft.Coordinate[1] - 0.02;

            var points = new List<Point>();
            for (int i = 0; i < innerPointsCount; i++)
                points.Add(new Point(buttomLeft.Coordinate[0] + 0.01 + width * Rand.NextDouble(),
                    buttomLeft.Coordinate[1] + 0.01 + height * Rand.NextDouble()));

            for (int i = 0; i < eachBoundPointsCount; i++)
            {
                points.Add(new Point(buttomLeft.Coordinate[0] + width * Rand.NextDouble(), buttomLeft.Coordinate[1]));
                points.Add(new Point(buttomLeft.Coordinate[0] + width * Rand.NextDouble(), topRight.Coordinate[1]));
                points.Add(new Point(buttomLeft.Coordinate[0], buttomLeft.Coordinate[1] + height * Rand.NextDouble()));
                points.Add(new Point(topRight.Coordinate[0], buttomLeft.Coordinate[1] + height * Rand.NextDouble()));
            }

            return points.ToArray();
        }
        */
    }
}
