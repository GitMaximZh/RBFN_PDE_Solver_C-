using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using KleinGordonPDE;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork;
using RBFNetwork.Function;
using RBFNetwork.Train;
using KleinGordonPDE.TrustRegion;

namespace Iteration.Solver
{
    class Program
    {
        private static readonly Random Rand = new Random(15123);

        private static readonly RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(1),
                WeightSettings = new ParameterSettings { value = 0, deviation = 0.5 },

                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.2 } }),

                StartPoint = new Point(-1.0),
                EndPoint = new Point(1.0),
                //CenterXDimentions = new List<int>(new[] { 10 })
            };
        private static readonly Func<IPoint, double> Solution =
           p => p.Coordinate[0] * Math.Cos(p.Coordinate[1]);


        static void Main(string[] args)
        {

            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.000000001 }, new TrustMethodFactory { PGP = true });
            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 12;


            var factory = new CaseFactory(new ProblemFactory(settings),
                new TrainFactory(optimizer),
                new ControlPointsFactory()
                {
                    C1Alfa = 100,
                    C2Alfa = 100,
                    C3Alfa = 100,
                    C4Alfa = 100,
                    ButtomLeftCorner = new Point(-1, 0),
                    TopRightCorner = new Point(1, 2),
                    XDimention = 22,
                    YDimention = 22,
                    Cf = 3
                });
            var ourCase = factory.Create();
            
        }
    }
}
