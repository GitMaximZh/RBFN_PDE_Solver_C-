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

namespace KleinGordonPDE.Discrete
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static readonly RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 1,
                FunctionCreator = () => new GaussianFunction(1),
                WeightSettings = new ParameterSettings { value = 0, deviation = 0.5 },

                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.2 } }),

                StartPoint = new Point(-1.0),
                EndPoint = new Point(1.0),
                //CenterXDimentions = new List<int>(new[] { 10, 10 })
            };
        
        public static Case Case_SVD(double t, double dt, Problem un, Problem un_1, double a)
        {
            var optimizer = new SVDOptimizer();
            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.ParametersSettings = new List<ParameterSettings>(new[] {new ParameterSettings {value = a}});
            settings.CenterXDimentions = new List<int>(new[] { 100 });
            //settings.FunctionsCount = 5;
            

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 1,
                    C2Alfa = 1,
                    LeftCorner = settings.StartPoint,
                    RightCorner = settings.EndPoint,
                    XDimention = 100,
                    UN = un,
                    UN_1 = un_1,
                    dT = dt,
                    T = t
                });
            var ourCase = factory.Create();
            //if(un != null)
            //{
            //    var pN = un.Network;
            //    var cN = ourCase.Problem.Network;
            //    cN.MoveToPoint(pN.GetPoint(optimizer.ParametersToOptimize.NetworkParameters), optimizer.ParametersToOptimize.NetworkParameters);
            //}
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            //new ControlPointsFactory()
            //{
            //    LeftCorner = settings.StartPoint,
            //    RightCorner = settings.EndPoint,
            //    C1Alfa = 1,
            //    C2Alfa = 1,
            //    XDimention = 25,
            //}.Create(ourCase.Problem);
            ourCase.Solution = p => p.Coordinate[0] * Math.Cos(t);
            return ourCase;
        }

        public static Case Case_TR(double t, double dt, Problem un, Problem un_1)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory { PGP = true });
            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 15;


            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 1000,
                    C2Alfa = 1000,
                    LeftCorner = settings.StartPoint,
                    RightCorner = settings.EndPoint,
                    XDimention = 45,
                    UN = un,
                    UN_1 = un_1,
                    dT = dt,
                    T = t
                });
            var ourCase = factory.Create();
            if (un != null)
            {
                var pN = un.Network;
                var cN = ourCase.Problem.Network;
                cN.MoveToPoint(pN.GetPoint(new NetworkParameters(false, true, true)), new NetworkParameters(false, true, true));
            }
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            //new ControlPointsFactory()
            //{
            //    LeftCorner = settings.StartPoint,
            //    RightCorner = settings.EndPoint,
            //    C1Alfa = 1,
            //    C2Alfa = 1,
            //    XDimention = 25,
            //}.Create(ourCase.Problem);
            ourCase.Solution = p => p.Coordinate[0] * Math.Cos(t);
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
