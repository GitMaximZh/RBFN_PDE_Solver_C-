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

namespace InverseProblem2.Inverse
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.05 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.3, deviation = 0.15 } }),
                StartPoint = new Point(0.0, 0.0),
                EndPoint = new Point(1.0, 1.0)
            };

        private static RBFNetworkSettings CoeffSettings =
            new RBFNetworkSettings()
            {
                Dimention = 1,
                FunctionsCount = 3,
                FunctionCreator = () => new GaussianFunction(1),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.05 },
                ParametersSettings =
                    new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.05 } }),
                StartPoint = new Point(0.0),
                EndPoint = new Point(1.0),
            };
        
        public static Case Case1(RBFNetwork.RBFNetwork direct)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = new CoefficientProblemParameters
                                                 {
                                                     NetworkParameters = new NetworkParameters(true, true, true),
                                                     ApproximatorParameters = new NetworkParameters(true, true, true)
                                                 };

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 16;

            var factory = new CaseFactory(new CoefficientProblemFactory(settings, CoeffSettings),
                new TrainFactory(optimizer), 
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    Beta = 100,
                    ButtomLeftCorner = new Point(0.0, 0.0),
                    TopRightCorner = new Point(1.0, 1.0),
                    XDimention = 12,
                    YDimention = 12,
                    DirectApproximator = direct
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).ToArray();
            ourCase.Solution = p => direct.Value(p.Coordinate);
            return ourCase;
        }
    }
}
