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
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,

                //FunctionsCount = 8,
                FunctionCreator = () => new WendlandFunction(2),
                NetworkType = NetworkType.Base,

                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.02 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.15 } }),

                //WeightSettings = new ParameterSettings { value = 0, deviation = 0.5 },
                //ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.05 } }),

                StartPoint = new Point(-0.2, -0.2),
                EndPoint = new Point(1.2, 1.2),
                CenterXDimentions = new List<int>(new [] { 6, 6 }) //для 10-5
            };

      

        #region Trust Region Method
        /// <summary>
        /// Нейронов: 16
        /// Перегенерация контрольных точек: да
        /// Алгоритм обучения: метод довер. обл.
        /// </summary>
        /// <returns></returns>
        public static Case Case_TR_1(RBFNetwork.RBFNetwork prevNetwork)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());


            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            //settings.FunctionsCount = 11; // для 10-4
            //settings.ParametersSettings[0] = new ParameterSettings {value = 0.3, deviation = 0.15};
            //settings.WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.05 };
            settings.Previous = prevNetwork;

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer/*, new ControlPointsTransformer
                                                {
                                                    ButtomLeftCorner = new Point(0, 0),
                                                    TopRightCorner = new Point(1, 1),
                                                    XSteps = 10,
                                                    YSteps = 10,
                                                    TransformEachXIteration = 1000
                                                }*/),
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0),
                    TopRightCorner = new Point(1, 1),
                    XDimention = 12,
                    YDimention = 12
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            ourCase.Solution =
                p => -1.0 * Math.Sin(Math.PI * p.Coordinate[0]) * Math.Sin(Math.PI * p.Coordinate[1]) / (2 * Math.Pow(Math.PI, 2));
            ourCase.CheckPoints = CheckPoints(100, 11, new Point(0, 0), new Point(1, 1));
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
