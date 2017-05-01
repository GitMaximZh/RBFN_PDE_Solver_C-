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
using RBFNetwork.Common;
using RBFNetwork.Function;
using RBFNetwork.Train;

namespace EITProblem.Inverse
{
    public static class InverseSuite
    {
        private static readonly Random Rand = new Random(15123);
        
        private static RBFNetworkSettings DefaultSettings =
            new RBFNetworkSettings()
            {
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.5 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.3, deviation = 0.15 } }),
                StartPoint = new Point(-1.0, -1.0),
                EndPoint = new Point(1.0, 1.0)
            };

        private static RBFNetworkSettings CoefficientSettings =
            new RBFNetworkSettings
            {
                NetworkType = NetworkType.Base,
                Dimention = 2,
                FunctionCreator = () => new GaussianFunction(2),
                WeightSettings = new ParameterSettings { value = 0.0, deviation = 0.5 },
                ParametersSettings = new List<ParameterSettings>(new[] { new ParameterSettings { value = 0.3, deviation = 0.15 } }),
                StartPoint = new Point(-1.0, -1.0),
                EndPoint = new Point(1.0, 1.0),
                FunctionsCount = 3
            };

        public static Case Case1(RBFNetwork.RBFNetwork directCV1, RBFNetwork.RBFNetwork coefficientV0)
        {
            var optimizer = new Optimizer(new StopStrategy { Epsilon = 0.00000001 }, new TrustMethodFactory());
            optimizer.ParametersToOptimize = new CoefficientProblemParameters
                                                 {
                                                     NetworkParameters = new NetworkParameters(true, true, true),
                                                     ApproximatorParameters = new NetworkParameters(true, true, true)
                                                 };

            var settings = (RBFNetworkSettings)DefaultSettings.Clone();
            settings.FunctionsCount = 5;

            var openWeight = 1.0;
            var electrodeWeight = 30.0;
            var factory = new CaseFactory(new CoefficientProblemFactory(directCV1, CoefficientSettings),
                new TrainFactory(optimizer), 
                new ControlPointsFactory(new Model.Model(
                    //Top
                    new Side(new OpenPart(new DenseVector(new[] { -1.0 / Math.Sqrt(2), 1.0 / Math.Sqrt(2) }), 
                                          openWeight, new Point(-1.0, 1), new Point(-1.0, 1), 1),
                             new OpenPart(new DenseVector(new[] { 0.0, 1.0 }),
                                          openWeight, new Point(-1.0, 1), new Point(-0.625, 1), 4),
                             new Electrode(new DenseVector(new[] { 0.0, 1.0 }), 
                                          electrodeWeight, new Point(-0.625, 1), new Point(-0.375, 1), 4, 10),                      
                             new OpenPart(new DenseVector(new[] { 0.0, 1.0 }),
                                          openWeight, new Point(-0.375, 1), new Point(0.375, 1), 7),
                             new Electrode(new DenseVector(new[] { 0.0, 1.0 }), 
                                          electrodeWeight, new Point(0.375, 1), new Point(0.625, 1), 4, -10),                      
                             new OpenPart(new DenseVector(new[] { 0.0, 1.0 }),
                                          openWeight, new Point(0.625, 1), new Point(1.0, 1), 4),
                             new OpenPart(new DenseVector(new[] { 1.0 / Math.Sqrt(2), 1.0 / Math.Sqrt(2) }),
                                          openWeight, new Point(1.0, 1), new Point(1.0, 1), 1)),

                    //Buttom
                    //new Side(new OpenPart(new DenseVector(new[] { -1.0 / Math.Sqrt(2), -1.0 / Math.Sqrt(2) }),
                    //                      openWeight, new Point(-1.0, -1), new Point(-1.0, -1), 1),
                    //         new OpenPart(new DenseVector(new[] { 0.0, -1.0 }),
                    //                      openWeight, new Point(-1.0, -1), new Point(1.0, -1), 10),
                    //         new OpenPart(new DenseVector(new[] { 1.0 / Math.Sqrt(2), -1.0 / Math.Sqrt(2) }),
                    //                      openWeight, new Point(1.0, -1), new Point(1.0, -1), 1)),
                    new Side(new OpenPart(new DenseVector(new[] { -1.0 / Math.Sqrt(2), -1.0 / Math.Sqrt(2) }),
                                          openWeight, new Point(-1.0, -1), new Point(-1.0, -1), 1),
                             new OpenPart(new DenseVector(new[] { 0.0, -1.0 }),
                                          openWeight, new Point(-1.0, -1), new Point(-0.625, -1), 4),
                             new Electrode(new DenseVector(new[] { 0.0, -1.0 }),
                                          electrodeWeight, new Point(-0.625, -1), new Point(-0.375, -1), 6, 10),
                             new OpenPart(new DenseVector(new[] { 0.0, -1.0 }),
                                          openWeight, new Point(-0.375, -1), new Point(0.375, -1), 7),
                             new Electrode(new DenseVector(new[] { 0.0, -1.0 }),
                                          electrodeWeight, new Point(0.375, -1), new Point(0.625, -1), 6, -10),
                             new OpenPart(new DenseVector(new[] { 0.0, -1.0 }),
                                          openWeight, new Point(0.625, -1), new Point(1.0, -1), 4),
                             new OpenPart(new DenseVector(new[] { 1.0 / Math.Sqrt(2), -1.0 / Math.Sqrt(2) }),
                                          openWeight, new Point(1.0, -1), new Point(1.0, -1), 1)),

                    //Left
                    //new Side(new OpenPart(new DenseVector(new[] { -1.0, 0.0 }),
                    //                      openWeight, new Point(-1.0, -1), new Point(-1.0, 1), 10)),

                    new Side(new OpenPart(new DenseVector(new[] { -1.0, 0.0 }),
                                          openWeight, new Point(-1.0, -1), new Point(-1.0, -0.625), 4),
                             new Electrode(new DenseVector(new[] { -1.0, 0.0 }),
                                          electrodeWeight, new Point(-1.0, -0.625), new Point(-1.0, -0.375), 6, -10),
                             new OpenPart(new DenseVector(new[] { -1.0, 0.0 }),
                                          openWeight, new Point(-1.0, -0.375, 1), new Point(-1.0, 0.375, 1), 7),
                             new Electrode(new DenseVector(new[] { -1.0, 0.0 }),
                                          electrodeWeight, new Point(-1.0, 0.375), new Point(-1.0, 0.625), 6, 10),
                             new OpenPart(new DenseVector(new[] { -1.0, 0.0 }),
                                          openWeight, new Point(-1.0, 0.625), new Point(-1.0, 1), 4)),

                    //Right
                    //new Side(new OpenPart(new DenseVector(new[] { 1.0, 0.0 }),
                    //                       openWeight, new Point(1.0, -1), new Point(1.0, 1), 10))))

                    new Side(new OpenPart(new DenseVector(new[] { 1.0, 0.0 }),
                                          openWeight, new Point(1.0, 1), new Point(1.0, 0.625), 4),
                             new Electrode(new DenseVector(new[] { 0.0, 1.0 }),
                                          electrodeWeight, new Point(1.0, 0.625), new Point(1.0, 0.375), 6, -10),
                             new OpenPart(new DenseVector(new[] { 1.0, 0.0 }),
                                          openWeight, new Point(1.0, 0.375, 1), new Point(1.0, -0.375, 1), 7),
                             new Electrode(new DenseVector(new[] { 1.0, 0.0 }),
                                          electrodeWeight, new Point(1.0, -0.375), new Point(1.0, -0.625), 6, 10),
                             new OpenPart(new DenseVector(new[] { 1.0, 0.0 }),
                                          openWeight, new Point(1.0, -0.625), new Point(1.0, -1), 4))))
                {
                    ButtomLeftCorner = new Point(-1.0, -1.0),
                    TopRightCorner = new Point(1.0, 1.0),
                    XDimention = 12,
                    YDimention = 12,
                    UMeasurements = CreateMeasurement(directCV1)
                });
            var ourCase = factory.Create();

            //top
            var f11 = new GaussianFunction(2);
            f11.Center[0].Value = -0.625;
            f11.Center[1].Value = 1.1;
            f11.Parameters[0].Value = 0.2;
            var f12 = new GaussianFunction(2);
            f12.Center[0].Value = -0.5;
            f12.Center[1].Value = 0.8;
            f12.Parameters[0].Value = 0.2;
            var f13 = new GaussianFunction(2);
            f13.Center[0].Value = -0.375;
            f13.Center[1].Value = 1.1;
            f13.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, f11);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -1.0 }, f12);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, f13);

            var f21 = new GaussianFunction(2);
            f21.Center[0].Value = 0.625;
            f21.Center[1].Value = 1.1;
            f21.Parameters[0].Value = 0.2;
            var f22 = new GaussianFunction(2);
            f22.Center[0].Value = 0.5;
            f22.Center[1].Value = 0.8;
            f22.Parameters[0].Value = 0.2;
            var f23 = new GaussianFunction(2);
            f23.Center[0].Value = 0.375;
            f23.Center[1].Value = 1.1;
            f23.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, f21);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 1.0 }, f22);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, f23);

            //right
            var g11 = new GaussianFunction(2);
            g11.Center[0].Value = 1.1;
            g11.Center[1].Value = -0.625;
            g11.Parameters[0].Value = 0.2;
            var g12 = new GaussianFunction(2);
            g12.Center[0].Value = 0.8;
            g12.Center[1].Value = -0.5;
            g12.Parameters[0].Value = 0.2;
            var g13 = new GaussianFunction(2);
            g13.Center[0].Value = 1.1;
            g13.Center[1].Value = -0.375;
            g13.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, g11);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 1.0 }, g12);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, g13);

            var g21 = new GaussianFunction(2);
            g21.Center[0].Value = 1.1;
            g21.Center[1].Value = 0.625;
            g21.Parameters[0].Value = 0.2;
            var g22 = new GaussianFunction(2);
            g22.Center[0].Value = 0.8;
            g22.Center[1].Value = 0.5;
            g22.Parameters[0].Value = 0.2;
            var g23 = new GaussianFunction(2);
            g23.Center[0].Value = 1.1;
            g23.Center[1].Value = 0.375;
            g23.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, g21);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -1.0 }, g22);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, g23);

            //left
            var l11 = new GaussianFunction(2);
            l11.Center[0].Value = -1.1;
            l11.Center[1].Value = -0.625;
            l11.Parameters[0].Value = 0.2;
            var l12 = new GaussianFunction(2);
            l12.Center[0].Value = -0.8;
            l12.Center[1].Value = -0.5;
            l12.Parameters[0].Value = 0.2;
            var l13 = new GaussianFunction(2);
            l13.Center[0].Value = -1.1;
            l13.Center[1].Value = -0.375;
            l13.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, l11);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -1.0 }, l12);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, l13);

            var l21 = new GaussianFunction(2);
            l21.Center[0].Value = -1.1;
            l21.Center[1].Value = 0.625;
            l21.Parameters[0].Value = 0.2;
            var l22 = new GaussianFunction(2);
            l22.Center[0].Value = -0.8;
            l22.Center[1].Value = 0.5;
            l22.Parameters[0].Value = 0.2;
            var l23 = new GaussianFunction(2);
            l23.Center[0].Value = -1.1;
            l23.Center[1].Value = 0.375;
            l23.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, l21);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 1.0 }, l22);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, l23);

            //bottom
            var b11 = new GaussianFunction(2);
            b11.Center[0].Value = -0.625;
            b11.Center[1].Value = -1.1;
            b11.Parameters[0].Value = 0.2;
            var b12 = new GaussianFunction(2);
            b12.Center[0].Value = -0.5;
            b12.Center[1].Value = -0.8;
            b12.Parameters[0].Value = 0.2;
            var b13 = new GaussianFunction(2);
            b13.Center[0].Value = -0.375;
            b13.Center[1].Value = -1.1;
            b13.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, b11);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 1.0 }, b12);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = 3.0 }, b13);

            var b21 = new GaussianFunction(2);
            b21.Center[0].Value = 0.625;
            b21.Center[1].Value = -1.1;
            b21.Parameters[0].Value = 0.2;
            var b22 = new GaussianFunction(2);
            b22.Center[0].Value = 0.5;
            b22.Center[1].Value = -0.8;
            b22.Parameters[0].Value = 0.2;
            var b23 = new GaussianFunction(2);
            b23.Center[0].Value = 0.375;
            b23.Center[1].Value = -1.1;
            b23.Parameters[0].Value = 0.2;
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, b21);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -1.0 }, b22);
            ourCase.Problem.Network.AddFunction(new Parameter { Value = -3.0 }, b23);

            ourCase.GraphicPoints = Utils.GetGraphicPoints(new Point(-1.0, -1.0), new Point(1.0, 1.0), 20, 20);
            ourCase.CheckPoints = Utils.GetGraphicPoints(new Point(-1.0, -1.0), new Point(1.0, 1.0), 9, 9);
            //ourCase.GraphicPoints = ourCase.ControlPoints.Select(p => (IControlPoint)p.Clone()).Where(p => p.Tag == 0).ToArray();
            ourCase.Solution = p => directCV1.Compute(p.Coordinate);
            return ourCase;
        }

        private static IList<Tuple<Electrode, Electrode, double>> CreateMeasurement(RBFNetwork.RBFNetwork network)
        {
            var pairs = new List<Tuple<Electrode, Electrode>>(new []
            {
                new Tuple<Electrode, Electrode>(
                    new Electrode(new DenseVector(new[] { 0.0, 1.0 }), 0, new Point(-0.625, 1), new Point(-0.375, 1), 4, 0),                      
                    new Electrode(new DenseVector(new[] { 0.0, 1.0 }), 0, new Point(0.375, 1), new Point(0.625, 1), 4, 0)), 

                new Tuple<Electrode, Electrode>(
                    new Electrode(new DenseVector(new[] { 1.0, 0.0 }), 0, new Point(1, 0.625), new Point(1, 0.375), 4, 0), 
                    new Electrode(new DenseVector(new[] { 1.0, 0.0 }), 0, new Point(1, -0.375), new Point(1, -0.625), 4, 0)),                                                                

                new Tuple<Electrode, Electrode>(
                    new Electrode(new DenseVector(new[] { 0.0, -1.0 }), 0, new Point(0.625, -1), new Point(0.375, -1), 4, 0), 
                    new Electrode(new DenseVector(new[] { 0.0, -1.0 }), 0, new Point(-0.375, -1), new Point(-0.625, -1), 4, 0)),

                new Tuple<Electrode, Electrode>(
                    new Electrode(new DenseVector(new[] { -1.0, 0.0 }), 0, new Point(-1.0, -0.625), new Point(-1.0, -0.375), 4, 0), 
                    new Electrode(new DenseVector(new[] { -1.0, 0.0 }), 0, new Point(-1.0, 0.375), new Point(-1.0, 0.625), 4, 0))
            });

            var result = new List<Tuple<Electrode, Electrode, double>>();
            foreach (var pair in pairs)
            {
                var e1 = pair.Item1;
                var e2 = pair.Item2;

                var sum = 0.0;
                double? a = null; double b;
                var step = e1.Length / e1.Density;

                foreach (var point in e1)
                {
                    b = network.Compute(point.Coordinate);
                    if (!a.HasValue)
                    {
                        a = b;
                        continue;
                    }
                    sum += 0.5 * step * (a.Value + b);
                    a = b;
                }

                a = null;
                step = e2.Length / e2.Density;
                foreach (var point in e2)
                {
                    b = network.Compute(point.Coordinate);
                    if (!a.HasValue)
                    {
                        a = b;
                        continue;
                    }
                    sum -= 0.5 * step * (a.Value + b);
                    a = b;
                }
                result.Add(new Tuple<Electrode, Electrode, double>(e1, e2, sum));
            }
            return result;
        }
    }
}
