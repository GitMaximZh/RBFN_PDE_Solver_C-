using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Common;
using ODE.Matlab;
using ODE.TrustRegion;
using PDE;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace ODE.Function
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
                {
                    Dimention = 1,

                    //FunctionsCount = 4,
                    FunctionCreator = () => new WendlandFunction(1),

                    WeightSettings = new ParameterSettings {value = 0, deviation = 0.5},

                    ParametersSettings =
                        new List<ParameterSettings>(new[] {new ParameterSettings {value = 1, deviation = 0.05}}),

                    StartPoint = new Point(0.0),
                    EndPoint = new Point(2*Math.PI),
                    CenterXDimentions = new List<int>(new[] { 15 })
                };

        
        public static Case Case_Sin()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new FminsearchMethodFactory());
            //optimizer.ParametersToOptimize = new NetworkParameters(true, true, 0, 1, 2, 3);

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.ParametersSettings = new List<ParameterSettings>(new[]
            {
                //A
                new ParameterSettings { value = 1},
                //f1
                //new ParameterSettings { value = 1 },
                //new ParameterSettings { value = 1 },
                // new ParameterSettings { value = 1 },
                //new ParameterSettings { value = 0 },
                //  //f1
                //new ParameterSettings { value = 0 },
                //     new ParameterSettings { value = 0 },
                //new ParameterSettings { value = 1 },
                //////  //f2
                //new ParameterSettings { value = 0.5 },
                ////////  //f2
                //new ParameterSettings { value = 0.5 },
                //new ParameterSettings { value = 0.5 },
                //////h1
                //new ParameterSettings { value = 0 },
                ////////h2
                //new ParameterSettings { value = 0 },
               
            });

            //settings.ParametersSettings[0] = new ParameterSettings { value = 0.45, deviation = 0.15 };

            //settings.ParametersSettings = new List<ParameterSettings>(new[]
            //{
            //    //A
            //    new ParameterSettings { value = 0, deviation = 0.5 },
            //    //Phi
            //    new ParameterSettings { value = 1 },
            //    ////Lambda
            //    new ParameterSettings { value = -0.577 },
            //    ////C1
            //    new ParameterSettings { value = 0 },
            //    ////C2
            //    new ParameterSettings { value = 0 }
            //});

            settings.WeightSettings = new ParameterSettings { value = 1.0, deviation = 0.05 };

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer), new FunctionControlPointsFactory
                {
                    Function = x => Math.Sin(x),
                    Left = new Point(0.0),
                    Right = new Point(2 * Math.PI),
                    XDimention = 90
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = new FunctionControlPointsFactory
                {
                    Function = x => Math.Sin(x),
                    Left = new Point(0.0),
                    Right = new Point(2 * Math.PI),
                    XDimention = 120
                }.Create().ToArray();
            ourCase.Solution =
                p => Math.Sin(p.Coordinate[0]);
            return ourCase;
        }
    }
}
