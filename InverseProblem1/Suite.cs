using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using InverseProblem1.TrustRegion;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace InverseProblem1
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 1,
                FunctionCreator = () => new GaussianFunction(1),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.05 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.15 } }),
                StartPoint = new Point(1.0),
                EndPoint = new Point(10.0)
            };

        private static RBFNetworkSettings CoeffSettings =
            new RBFNetworkSettings()
            {
                Dimention = 1,
                FunctionsCount = 3,
                FunctionCreator = () => new GaussianFunction(1),
                WeightSettings = new ParameterSettings { value = 0.5, deviation = 0.05 },
                ParametersSettings =
                    new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.05 } }),
                StartPoint = new Point(1.0),
                EndPoint = new Point(10.0),
            };

      

        #region Trust Region Method
        /// <summary>
        /// Нейронов: 16
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// </summary>
        /// <returns></returns>
        public static Case Case_1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = new CoefficientProblemParameters
                                                 {
                                                     ApproximatorParameters = new NetworkParameters(true, true, true),
                                                     NetworkParameters = new NetworkParameters(true, true, true)
                                                 };

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 5;
            
            var factory = new CaseFactory(new CoefficientProblemFactory(settings, CoeffSettings),
                new TrainFactory(optimizer), 
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(1.0),
                    TopRightCorner = new Point(10.0),
                    XDimention = 55
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            ourCase.Solution =
                p => Math.Pow(p.Coordinate[0], 3);
           // ourCase.CheckPoints = CheckPoints(100, 11, new Point(0, 0), new Point(1, 1));
            return ourCase;
        }

       
        #endregion


        private static Point[] CheckPoints(int innerPointsCount, int eachBoundPointsCount, Point buttomLeft, Point topRight)
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
