using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using ODE.TrustRegion;
using PDE;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Function;

namespace ODE.ODE2
{
    public static class Suite
    {
        private static readonly Random Rand = new Random(15123);

        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 1,

                FunctionsCount = 3,
                FunctionCreator = () => new SplineFunction(1, 2),

                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.5, deviation = 0.05 } }),

                StartPoint = new Point(0.0),
                EndPoint = new Point(2.0 * Math.PI),
                //CenterXDimentions = new List<int>(new[] { 9 })
            };


        public static Case Case_ODE1()
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            //optimizer.ParametersToOptimize = new NetworkParameters(true, true, 0, 1, 2, 3);

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();

            settings.ParametersSettings[0] = new ParameterSettings { value = 0.45, deviation = 0.15 };

            settings.ParametersSettings = new List<ParameterSettings>(new[]
            {
                //A
                new ParameterSettings { value = 1, deviation = 0.5 },
                ////Phi
                new ParameterSettings { value = 1 },
                ////Lambda
                new ParameterSettings { value = -1 },
                ////C1
                new ParameterSettings { value = 0.7 },
                ////C2
                new ParameterSettings { value = 0 }
            });

            settings.WeightSettings = new ParameterSettings { value = 1.0, deviation = 0.05 };

            var factory = new CaseFactory(new RBFNetworkFactory(settings),
                new TrainFactory(optimizer), new ControlPointsFactory()
                {
                    Left = new Point(0.0),
                    Right = new Point(2.0 * Math.PI),
                    XDimention = 160,
                    Alfa = 300
                });
            var ourCase = factory.Create();
            ourCase.GraphicPoints = GraphicPoints(50, new Point(0.0), new Point(2.0 * Math.PI));
            ourCase.Solution =
                p => 0.1 * Math.Cos(4 * p[0]) - 0.1 * Math.Sin(4 * p[0]) + 1.0 / 12.0 * Math.Cos(2 * p[0]) + 1.0 / 16.0;
            return ourCase;
        }

        private static Point[] GraphicPoints(int innerPointsCount, Point left, Point right)
        {
            var points = new List<Point>();
            var step = (right.Coordinate[0] - left.Coordinate[0]) / (innerPointsCount + 1);

            points.Add(left);


            for (int i = 0; i < innerPointsCount; i++)
                points.Add(new Point(left.Coordinate[0] + (i + 1) * step));

            points.Add(right);
            return points.ToArray();
        }
    }
}
