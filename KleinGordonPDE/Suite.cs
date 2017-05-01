using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using KleinGordonPDE.TrustRegion;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace KleinGordonPDE
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static readonly RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2),
                WeightSettings = new ParameterSettings { value = 0, deviation = 0.5 },

                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.326 } }),

                StartPoint = new Point(-1, 0),
                EndPoint = new Point(1, 2),
                //CenterXDimentions = new List<int>(new[] { 10, 10 })
            };


        private static readonly Func<IPoint, double> Solution =
            p => p.Coordinate[0] * Math.Cos(p.Coordinate[1]);

        public static Case Case_TR_1(int step, Problem problem)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory { PGP = true });
            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 12;
            settings.StartPoint = new Point(-1, 2.0 * step);
            settings.EndPoint = new Point(1, 2.0 * (step + 1));

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 20,
                    C2Alfa = 10,
                    C3Alfa = 20,
                    C4Alfa = 20,
                    ButtomLeftCorner = settings.StartPoint,
                    TopRightCorner = settings.EndPoint,
                    XDimention = 7,
                    YDimention = 7,
                    Cf = 3,
                    PreviousProblem = problem
                });
            var ourCase = factory.Create();
            //if(problem != null)
            //{
            //    var pN = problem.Network;
            //    var cN = ourCase.Problem.Network;
            //    cN.MoveToPoint(pN.GetPoint(optimizer.ParametersToOptimize.NetworkParameters), optimizer.ParametersToOptimize.NetworkParameters);
            //}
            ourCase.GraphicPoints = new ControlPointsFactory()
            {
                ButtomLeftCorner = settings.StartPoint,
                TopRightCorner = settings.EndPoint,
                C1Alfa = 1,
                C2Alfa = 1,
                C3Alfa = 1,
                C4Alfa = 1,
                XDimention = 25,
                YDimention = 25,
                Cf = 1
            }.Create(ourCase.Problem);
            ourCase.Solution = Solution;
            return ourCase;
        }

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
    }

}
