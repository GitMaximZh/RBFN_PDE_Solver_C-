using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using EITProblem.Model;
using EITProblem.TrustRegion;
using MathNet.Numerics.LinearAlgebra.Double;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Coefficient
{
    public static class CoefficientSuite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings
            {
                NetworkType = NetworkType.Normalized,
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.5 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.3, deviation = 0.15 } }),
                StartPoint = new Point(-1.0, -1.0),
                EndPoint = new Point(0.25, 0.25)
            };
        
        public static Case Case1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = new ProblemParameters
                                                 {
                                                     NetworkParameters = new NetworkParameters(true, true, true)
                                                 };

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 7;

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer), 
                new ControlPointsFactory
                {
                    ButtomLeftCorner = new Point(-1.0, -1.0),
                    TopRightCorner = new Point(0.25, 0.25),
                    XDimention = 21,
                    YDimention = 21
                });
            var ourCase = factory.Create();
            var cf = new ControlPointsFactory
                         {
                             ButtomLeftCorner = new Point(-1.0, -1.0),
                             TopRightCorner = new Point(0.25, 0.25),
                             XDimention = 21,
                             YDimention = 21
                         };
            cf.Create(ourCase.Problem);
            ourCase.GraphicPoints = cf.Create(ourCase.Problem).ToArray();
            ourCase.Solution = p =>
                                   {
                                       //var d = Math.Pow(p[0] - 0.5, 2) + Math.Pow(p[1] - 0.5, 2);
                                       //return d > 0.09 ? 1 : 0.2;

                                       var dBgd = Math.Pow(p[0], 2) + Math.Pow(p[1], 2);
                                       var degree18 = 2.0 * Math.PI * -18 / 360;
                                       var dLung = Math.Pow((p[0] + 0.5) * Math.Cos(degree18) - (p[1]) * Math.Sin(degree18), 2) / Math.Pow(0.38, 2) +
                                           Math.Pow((p[0] + 0.5) * Math.Sin(degree18) + (p[1]) * Math.Cos(degree18), 2) / Math.Pow(0.625, 2);
                                       var dSpine = Math.Pow(p[0], 2) + Math.Pow(p[1] + 0.65, 2);
                                       if (dLung <= 1)
                                           return 0.83;
                                       if (dSpine <= 0.0225)
                                           return 0.5;
                                       return 3.3;
                                   }; 
            return ourCase;
        }
    }
}
