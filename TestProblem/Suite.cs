using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Function;
using RBFNetwork.Train;
using TestProblem.TrustRegion;

namespace TestProblem
{
    public static class Suite
    {
        public const double FMean = 0.5;
        public const double FSD = 0.05;
        public const double Absorption = 3;

        public static Func<double[], double> I0 = p => 0;
        public static Func<double[], double> F =
            p => 1.0 / (FSD * Math.Sqrt(2.0 * Math.PI)) * Math.Exp(-(Math.Pow(p[0] - FMean, 2) + Math.Pow(p[1] - FMean, 2)) / (2 * FSD * FSD));

        public static Func<double[], double> F1 =
            p => p[1] > 0.1 ? 0.0 : 1.0;

        public static Func<double[], double> U1 =
          p => 0.5 * p[1];

        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 3,

                FunctionsCount = 10,
                FunctionCreator = () => new AsymmetricGaussianFunction(3),
                NetworkType = NetworkType.Base,

                WeightSettings = new ParameterSettings { value = 0.0, deviation = 3 },
                ParametersSettings = new List<ParameterSettings>(new[]
                {
                    new ParameterSettings { value = 0.3, deviation = 0.1 },
                    new ParameterSettings { value = 0.3, deviation = 0.1 }
                    //new ParameterSettings { value = 0.3, deviation = 0.1 }
                }),

                StartPoint = new Point(-0.1, -0.1, -0.1),
                EndPoint = new Point(1.1, 1.1, 1.1),
                //CenterXDimentions = new List<int>(new [] { 6, 6 }) //для 10-5
            };



        #region Trust Region Method
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static Case Case()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());


            var settings = (RBFNetworkSettings)DefaultSettings.Clone();

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0, 0, 0),
                    TopRightCorner = new Point(1, 1, 1),
                    XDimention = 8,
                    YDimention = 8,
                    TDimention = 12
                });

            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            ourCase.Solution = p => I0(p.Coordinate) * Math.Exp(-Absorption * p.Coordinate[2]) + 
                F(p.Coordinate) / Absorption * (1.0 - Math.Exp(-Absorption * p.Coordinate[2]));
            return ourCase;
        }

        #endregion
    }
}
