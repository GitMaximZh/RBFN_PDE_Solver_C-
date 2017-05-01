using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using EvolutionInverseProblem.TrustRegion;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EvolutionInverseProblem.Inverse
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
               ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.3, deviation = 0.15 }, 
                    /*new ParameterSettings { value = 0.03, deviation = 0.015 }*/}),
               StartPoint = new Point(0.0, -0.1),
               EndPoint = new Point(1.0, 0.1)
           };
        
        public static Case Case1(RBFNetwork.RBFNetwork direct)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = new ProblemParameters
                                                 {
                                                     NetworkParameters = new NetworkParameters(true, true, true)
                                                 };

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 16;

            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer), 
                new ControlPointsFactory
                {
                    Alfa = 1000,
                    ButtomLeftCorner = new Point(0.0, 0.0),
                    TopRightCorner = new Point(1.0, 0.1),
                    XDimention = 12,
                    YDimention = 12,
                    DirectApproximator = direct,
                    cX = 2,
                    cY = 3
                });
            var ourCase = factory.Create();
            ourCase.CheckPoints =
                ourCase.ControlPoints.ToArray();
            ourCase.GraphicPoints = new ControlPointsFactory
            {
                Alfa = 1000,
                ButtomLeftCorner = new Point(0.0, 0.0),
                TopRightCorner = new Point(1.0, 0.1),
                XDimention = 12,
                YDimention = 12,
                DirectApproximator = direct,
                cX = 1,
                cY = 1
            }.Create(ourCase.Problem).Select(p => (IControlPoint)p.Clone()).ToArray();
            ourCase.Solution = p => direct.Value(p.Coordinate);
            return ourCase;
        }
    }
}
