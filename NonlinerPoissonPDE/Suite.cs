using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using NonlinerPoissonPDE.TrustRegion;
using PDE;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace NonlinerPoissonPDE
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static readonly RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2)
            };

        private static readonly ControlPoint[] GraphicPoints = new ControlPointsFactory()
        {
            ButtomLeftCorner = new Point(0, 0),
            TopRightCorner = new Point(1, 2),
            Alfa = 1,
            XDimention = 25,
            YDimention = 25,
        }.Create();

        private static readonly Point[] CheckPoints =  new ControlPointsFactory()
        {
            ButtomLeftCorner = new Point(0, 0),
            TopRightCorner = new Point(1, 2),
            Alfa = 1,
            XDimention = 10,
            YDimention = 10,
        }.Create();

        private static readonly Func<Point, double> Solution =
            p => (1.0 - p.Coordinate[0] * p.Coordinate[0]) * Math.Sin(Math.PI * p.Coordinate[1] / 2.0);

     

        #region Trust Region Method
        /// <summary>
        /// Нейронов: 16
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// </summary>
        /// <returns></returns>
        public static Case Case_TR_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory());

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            //settings.FunctionsCount = 36;
            //width setting
            settings.ParametersSettings.Add(new ParameterSettings { Value = 0.34 });
            //settings.WidthDeviation1 = 0.15;
            settings.WeightSettings = new ParameterSettings {Value = 0};
            //settings.WeightDeviation = 0.05;
            settings.CentersButtomLeftCorner = new Point(-0.5, -0.5);
            settings.CentersTopRightCorner = new Point(1.5, 2.5);
            settings.CentersXDimention = 5;
            settings.CentersYDimention = 7;
            
            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    Alfa = 20,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 2),
                    XDimention = 12,
                    YDimention = 12,
                    Cf = 8
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = GraphicPoints;
            ourCase.Solution = Solution;
            ourCase.CheckPoints = CheckPoints;
            return ourCase;
        }

        /// <summary>
        /// Нейронов: 64
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// </summary>
        /// <returns></returns>
        public static Case Case_TR_2()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory());
            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            //settings.FunctionsCount = 36;
            //width setting
            settings.ParametersSettings.Add(new ParameterSettings { Value = 0.34 });
            //settings.WidthDeviation1 = 0.15;
            settings.WeightSettings = new ParameterSettings { Value = 0, Deviation = 0.5 };
            
            //settings.Width = 0.5;
            //settings.WidthDeviation = 0.05;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer,
                    new ControlPointsTransformer
                    {
                        ButtomLeftCorner = new Point(0, 0),
                        TopRightCorner = new Point(1, 1),
                        XSteps = 5,
                        YSteps = 5,
                        TransformEachXIteration = 500
                    }),
                new ControlPointsFactory()
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (ControlPoint)p.Clone()).ToArray();
            ourCase.Solution =
                p => -1.0 * Math.Sin(Math.PI * p.Coordinate[0]) * Math.Sin(Math.PI * p.Coordinate[1]) / (2 * Math.Pow(Math.PI, 2));
            ourCase.CheckPoints = GenerateCheckPoints(100, 11, new Point(0, 0), new Point(1, 1));
            return ourCase;
        }

        /// <summary>
        /// Нейронов: 64
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// Обучаются: только веса
        /// </summary>
        /// <returns></returns>
        public static Case Case_TR_4()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = NetworkParameters.W;

            var factory = new CaseFactory(new RBFNetworkFactory(DefaultSettings),
                new TrainFactory(optimizer,
                    new ControlPointsTransformer
                        {
                            ButtomLeftCorner = new Point(0, 0),
                            TopRightCorner = new Point(1, 1),
                            TransformEachXIteration = 2,
                            XSteps = 5,
                            YSteps = 5
                        }),
                    new ControlPointsFactory()
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (ControlPoint)p.Clone()).ToArray();
            ourCase.Solution =
                p => -1.0 * Math.Sin(Math.PI * p.Coordinate[0]) * Math.Sin(Math.PI * p.Coordinate[1]) / (2 * Math.Pow(Math.PI, 2));
            ourCase.CheckPoints = GenerateCheckPoints(100, 11, new Point(0, 0), new Point(1, 1));
            return ourCase;
        }

        /// <summary>
        /// Нейронов: 144
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// </summary>
        /// <returns></returns>
        public static Case Case_TR_3()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());

            var settings = DefaultSettings;
            settings.CentersButtomLeftCorner = new Point(0, 0);
            settings.CentersTopRightCorner = new Point(1, 1);
            settings.CentersXDimention = 12;
            settings.CentersYDimention = 12;
            //settings.Width = 0.2;

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer,
                    new ControlPointsTransformer
                    {
                        ButtomLeftCorner = new Point(0, 0),
                        TopRightCorner = new Point(1, 1),
                        XSteps = 5,
                        YSteps = 5,
                        TransformEachXIteration = 50
                    }),
                new ControlPointsFactory()
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (ControlPoint)p.Clone()).ToArray();
            ourCase.Solution =
                p => -1.0 * Math.Sin(Math.PI * p.Coordinate[0]) * Math.Sin(Math.PI * p.Coordinate[1]) / (2 * Math.Pow(Math.PI, 2));
            ourCase.CheckPoints = GenerateCheckPoints(100, 11, new Point(0, 0), new Point(1, 1));
            return ourCase;
        }

        #endregion

       

        private static Point[] GenerateCheckPoints(int innerPointsCount, int eachBoundPointsCount, Point buttomLeft, Point topRight)
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
    }
}
