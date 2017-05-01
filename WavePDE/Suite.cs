using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using PDE;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Function;
using RBFNetwork.Train;
using WavePDE.QuasiNewton;
using WavePDE.TrustRegion;

namespace WavePDE
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,

                FunctionsCount = 20,
                FunctionCreator = () => new GaussianFunction(2),

                Weight = 0,
                //WeightDeviation = 0.5,

                Width = 0.4,
                //WidthDeviation = 0.1,
                
                CentersButtomLeftCorner = new Point(-0.2, -0.1),
                CentersTopRightCorner = new Point(1.2, 0.6),
                //CentersXDimention = 11,
                //CentersYDimention = 6,
            };

        #region SVD

        public static Case Case_SVD_1(double width = 0.3)
        {
            var optimizer = new SVDOptimizer();

            var settings = DefaultSettings;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0;
            settings.Width = width;
            settings.WidthDeviation = 0;
            settings.CentersButtomLeftCorner = new Point(0, 0);
            settings.CentersTopRightCorner = new Point(1, 0.5);
            settings.CentersXDimention = 11;
            settings.CentersYDimention = 6;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 100,
                    C2Alfa = 20,
                    C3Alfa = 100,
                    C4Alfa = 15,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 14,
                    YDimention = 6
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            ourCase.CheckPoints = GraphicPoints(12, 12, new Point(0, 0), new Point(1, 0.5));
            ourCase.GraphicPoints = GraphicPoints(12, 12, new Point(0, 0), new Point(1, 0.5));
            return ourCase;
        }

        private static Point[] GraphicPoints(int xDimention, int yDimention, Point buttomLeft, Point topRight)
        {
            var points = new List<Point>();

            var xstep = (topRight.Coordinate[0]
                          - (buttomLeft.Coordinate[0])) / (xDimention - 1);
            var ystep = (topRight.Coordinate[1]
                          - buttomLeft.Coordinate[1]) / (yDimention - 1);

            for (int i = 0; i <= xDimention; i++)
            {
                for (int j = 0; j <= yDimention; j++)
                {
                    points.Add(new Point(buttomLeft.Coordinate[0] + xstep * i,
                                         buttomLeft.Coordinate[1] + ystep * j));
                }
            }

            return points.ToArray();
        }

        #endregion

        #region Like SVD Condition

        public static Case Case_BFGS_W_1()
        {
            var optimizer = new Optimizer(new StopStrategy {Epsilon = 0.00000001},
                                          new BFGSMethodFactory {MinStep = 0.000001, Speed = 0.001}) { ParametersToOptimize = NetworkParameters.W };
               
            var settings = DefaultSettings;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0;
            settings.Width = 0.4;
            settings.WidthDeviation = 0;
            settings.CentersButtomLeftCorner = new Point(0, 0);
            settings.CentersTopRightCorner = new Point(1, 0.5);
            settings.CentersXDimention = 8;
            settings.CentersYDimention = 4;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    C1Alfa = 100,
                    C2Alfa = 20,
                    C3Alfa = 100,
                    C4Alfa = 15,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 10,
                    YDimention = 10
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }

        public static Case Case_LM_W_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new LevenbergMarquardtMethodFactory { MinStep = 0.000001, Tau = 0.1 });
            //optimizer.ParametersToOptimize = NetworkParameters.W;
            var settings = DefaultSettings;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0;
            settings.Width = 0.4;
            settings.WidthDeviation = 0;
            settings.CentersButtomLeftCorner = new Point(0, 0);
            settings.CentersTopRightCorner = new Point(1, 0.5);
            settings.CentersXDimention = 10;
            settings.CentersYDimention = 10;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    C1Alfa = 100,
                    C2Alfa = 20,
                    C3Alfa = 100,
                    C4Alfa = 15,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 10,
                    YDimention = 10
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }

        public static Case Case_TR_W_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory());
            //optimizer.ParametersToOptimize = NetworkParameters.W;
            var settings = DefaultSettings;
            settings.Weight = 0.0;
            settings.WeightDeviation = 0;
            settings.Width = 0.4;
            settings.WidthDeviation = 0;
            settings.CentersButtomLeftCorner = new Point(0, 0);
            settings.CentersTopRightCorner = new Point(1, 0.5);
            settings.CentersXDimention = 10;
            settings.CentersYDimention = 10;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    C1Alfa = 100,
                    C2Alfa = 20,
                    C3Alfa = 100,
                    C4Alfa = 15,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 10,
                    YDimention = 10
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }

        #endregion

        public static Case Case_TR_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory());

            var settings = DefaultSettings;
            settings.Weight = 0;
            //settings.WeightDeviation = 0.1;
            settings.Width = 0.9;//.7;
            //settings.WidthDeviation = 0.05;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 100,
                    C2Alfa = 20,
                    C3Alfa = 100,
                    C4Alfa = 15,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 12,
                    YDimention = 12,
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }

        public static Case Case_LM_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new LevenbergMarquardtMethodFactory { MinStep = 0.000001, Tau = 0.1 });
            var settings = DefaultSettings;
            settings.Weight = 0;
            //settings.WeightDeviation = 0.1;
            settings.Width = 0.7;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 100,
                    C2Alfa = 100,
                    C3Alfa = 100,
                    C4Alfa = 100,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 11,
                    YDimention = 11,
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }

        public static Case Case_BFGS_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new BFGSMethodFactory { MinStep = 0.000001, Speed = 0.001 });
            var settings = DefaultSettings;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 1000,
                    C2Alfa = 1000,
                    C3Alfa = 1000,
                    C4Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 0.5),
                    XDimention = 11,
                    YDimention = 11,
                });
            var ourCase = factory.Create();
            ourCase.Solution =
                p => Math.Cos(Math.PI * p[0]) * Math.Sin(Math.PI * p[1]) - (1.0 - 2.0 * p[0] / 1.0) * Math.Sin(Math.PI * p[1]);
            return ourCase;
        }
    }

}
