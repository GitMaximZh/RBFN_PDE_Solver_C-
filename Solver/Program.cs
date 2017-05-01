using System;
using System.Collections.Generic;
using Common;
using PDE.Solution;
using RBFNetwork.Tools;
using RBFNetwork.Train;
using Collectors = PDE.Statistic;
using Solver.Statistic;


namespace Solver
{
    internal class Program
    {
        static string path = "E:\\tp1";

        private static void Main(string[] args)
        {
            //MultiLayerSolver();
            //ForReport();
            //Solver();

            Show();
        }

        private static void Show()
        {
            var nw = NetworkFactory.Instance.Create("E:\\tp1607.xml");
            var currentCase = TestProblem.Suite.Case();
            var statistic = new PDE.Statistic.Statistic();

            var ButtomLeftCorner = new Point(0, 0, 0);
            var TopRightCorner = new Point(1, 1, 1);

            var points = new List<IPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (15 - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (15 - 1);
            var tstep = (TopRightCorner.Coordinate[2]
                          - ButtomLeftCorner.Coordinate[2]) / (15 - 1);

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    //for (int k = 1; k < 15; k++)
                    {
                        points.Add(new Point(ButtomLeftCorner.Coordinate[0] + xstep * i,
                            ButtomLeftCorner.Coordinate[1] + tstep * j, 1.0));
                    }
                }
            }

            var result = new Collectors.NetworkGrapthicCollector(points.ToArray(), 0);
            var g = new ShowGraphic(result);
            //g.ConfigurationScript = "3d_config";
            g.GrapthicScript = "Contour" + 0;
            g.DataFile = "network_graphic" + 0;
            
            statistic.AddStatisticCollector("Main", result);
            statistic.Collect("Main", new Trainer.TrainStateArg() { Problem = new Problem(nw)});
            statistic.ExecuteExtention<IShowExtention>("Main");
            Console.ReadLine();
        }

        private static void MultiLayerSolver()
        {
            Case ourCase1 = PoissonPDE.Suite.Case_TR_1(NetworkFactory.Instance.Create(@"E:\PoissonNetwork.xml"));
            //ourCase1.Steps = 2;

            var statistic = new PDE.Statistic.Statistic();
            StatisticBuilder.Build(statistic, ourCase1);

            Trainer.TrainStateArg lastTrainState = null;

            ourCase1.Trainer.IterationFinished += (s, e) =>
            {
                lastTrainState = e;
                statistic.Collect("Iteration", e);
                statistic.Collect("IterationCollect&TrainShow", e);
                statistic.ExecuteExtention<IShowExtention>("Iteration");
                Displayer.Instance.Display();
                if(e.Iteration == 70)
                    ourCase1.Trainer.BreakTrain();
            };

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                ourCase1.Trainer.BreakTrain();
            };

            ourCase1.Trainer.Train(ourCase1.Steps);

            if (lastTrainState != null)
            {
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
                //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            }

            
            NetworkFactory.Instance.Save(@"E:\PoissonNetwork.xml", ourCase1.Problem.Network);

            Console.ReadLine();
        }

        /*
        private static void Research()
        {
            var ourCase = ChoiseCase();

            var statistic = new PDE.Statistic.Statistic();
            var factory = new StaticticCollectorFactory(ourCase);
            statistic.AddStatisticCollector("Iteration", factory.CreateIterationInfoCollector());
            var rCs = new List<RelativeErrorCollector>();
            var r1 = factory.CreateRelativeErrorCollector();
            statistic.AddStatisticCollector("Iteration", r1);
            rCs.Add(r1);
            if (ourCase.CheckPoints != null)
            {
                var r2 = factory.CreateRelativeErrorCollector(ourCase.CheckPoints);
                statistic.AddStatisticCollector("Iteration", r2);
                rCs.Add(r2);
            }
            statistic.AddStatisticCollector("IterationCollect&TrainShow",
                                            factory.CreateRelativeErrorHistoryGraphicCollector(rCs.ToArray()));
            int iteration = 1;
            Trainer.TrainStateArg lastTrainState = null;
            ourCase.Trainer.IterationFinished += (s, e) =>
                                                     {

                                                         e.IsLastIteration = false;
                                                         e.Iteration = iteration;
                                                         lastTrainState = e;
                                                         statistic.Collect("Iteration", e);
                                                         statistic.Collect("IterationCollect&TrainShow", e);
                                                         statistic.ExecuteExtention<IShowExtention>("Iteration");
                                                         Displayer.Instance.Display();
                                                     };

            for (double a = 0.1; a < 1; a += 0.02)
            {
                Console.Write("A: {0} ", a);

                ourCase.Network.MoveToPoint(new Point(Enumerable.Repeat(0.0, ourCase.Network.HiddenCount).ToArray()),
                                            NetworkParameters.W);
                ourCase.Network.MoveToPoint(new Point(Enumerable.Repeat(a, ourCase.Network.HiddenCount).ToArray()),
                                            NetworkParameters.A);
                ourCase.Trainer.Train(ourCase.Steps);
                iteration++;
            }

            if (lastTrainState != null)
            {
                lastTrainState.IsLastIteration = true;
                statistic.Collect("IterationCollect&TrainShow", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            }


            Console.ReadLine();
        }
        */

        private static void ForReport()
        {
            Case _case = EITProblem.Coefficient.CoefficientSuite.Case1();

            var statistic = new PDE.Statistic.Statistic();
            StatisticBuilder.BuildForReport(statistic, _case);

            Trainer.TrainStateArg lastTrainState = null;

            _case.Trainer.IterationFinished += (s, e) =>
            {
                lastTrainState = e;
                statistic.Collect("Iteration", e);
                statistic.Collect("IterationCollect&TrainShow", e);
                statistic.ExecuteExtention<IShowExtention>("Iteration");
                Displayer.Instance.Display();
                if(e.Error < 9.0)
                    _case.Trainer.BreakTrain();
            };

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
                _case.Trainer.BreakTrain();
            };

            _case.Trainer.Train(_case.Steps);

            if (lastTrainState != null)
            {
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
                //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            }
            Console.ReadLine();
        }

        private static void InverseEITSolver()
        {
            var directCV1 = NetworkFactory.Instance.Create("E:\\t.xml");
            var coefficientV0 = NetworkFactory.Instance.Create("E:\\Coefficient v1.xml");

            Case inverseCase = EITProblem.Inverse.InverseSuite.Case1(directCV1, coefficientV0);
            
            var statistic = new PDE.Statistic.Statistic();
            StatisticBuilder.BuildEIT(statistic, inverseCase);

            Trainer.TrainStateArg lastTrainState = null;

            inverseCase.Trainer.IterationFinished += (s, e) =>
                                                         {
                                                             lastTrainState = e;
                                                             statistic.Collect("Iteration", e);
                                                             statistic.Collect("IterationCollect&TrainShow", e);
                                                             statistic.ExecuteExtention<IShowExtention>("Iteration");
                                                             Displayer.Instance.Display();
                                                             //if(e.Error < 0.0001)
                                                             //    ourCase1.Trainer.BreakTrain();
                                                         };

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                                          {
                                              e.Cancel = true;
                                              statistic.Collect("Train", lastTrainState);
                                              statistic.ExecuteExtention<IShowExtention>("Train");
                                              //ourCase1.Trainer.BreakTrain();
                                          };

            inverseCase.Trainer.Train(inverseCase.Steps);

            if (lastTrainState != null)
            {
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
                //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            }
            Console.ReadLine();
        }

        private static void Solver()
        {
            //Case directCase = EITProblem.Direct.DirectSuite.Case1();
            //Case coefficientCase = EITProblem.Coefficient.CoefficientSuite.Case1();
            //ourCase1.Steps = 2;
            var currentCase = TestProblem.Suite.Case();
            var path = "E:\\tp1";

            var statistic = new PDE.Statistic.Statistic();
            StatisticBuilder.Build(statistic, currentCase);

            Trainer.TrainStateArg lastTrainState = null;

            currentCase.Trainer.IterationFinished += (s, e) =>
                                                     {
                                                         lastTrainState = e;
                                                         statistic.Collect("Iteration", e);
                                                         //statistic.Collect("IterationCollect&TrainShow", e);
                                                         statistic.ExecuteExtention<IShowExtention>("Iteration");
                                                         Displayer.Instance.Display();
                                                     };

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                                          {
                                              e.Cancel = true;
                                              statistic.Collect("Train", lastTrainState);
                                              statistic.ExecuteExtention<IShowExtention>("Train");
                                              NetworkFactory.Instance.Save(path + DateTime.Now.Millisecond + ".xml", currentCase.Problem.Network);
                                              //ourCase1.Trainer.BreakTrain();
                                          };

            currentCase.Trainer.Train(currentCase.Steps);

            if (lastTrainState != null)
            {
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
                NetworkFactory.Instance.Save(path + DateTime.Now.Millisecond + ".xml", currentCase.Problem.Network);
                //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            }
            Console.ReadLine();

            //Case ourCase2 = EvolutionInverseProblem.Inverse.Suite.Case1(ourCase1.Problem.Network);

            //statistic = new PDE.Statistic.Statistic();
            //StatisticBuilder.Build(statistic, ourCase2);
            //var factory = new StaticticCollectorFactory(ourCase2);

            //statistic.AddStatisticCollector("Train", factory.CreateActualSliceAndNetworkSliceGraphicCollector(p => 
            //    -2.0 * (p[0] - 0.5) * (p[0] - 0.5) + 0.5,
            //    //p[0] <= 0.3 ? p[0] / 0.3 : (1.0 - p[0]) / 0.7,
            //    Enumerable.Range(0, 101).Select(i => new Point(0.01 * i, 0)).ToArray()));
            //statistic.AddStatisticCollector("Train", factory.CreateActualSliceAndNetworkSliceGraphicCollector(p => ourCase1.Problem.Network.Compute(p.Coordinate),
            //    Enumerable.Range(0, 101).Select(i => new Point(0.01 * i, 0.05)).ToArray(), "1"));
            //statistic.AddStatisticCollector("Iteration", factory.CreateSliceRelativeErrorCollector(p =>
            //    -2.0 * (p[0] - 0.5) * (p[0] - 0.5) + 0.5,
            //    //p[0] <= 0.3 ? p[0] / 0.3 : (1.0 - p[0]) / 0.7,
            //    Enumerable.Range(0, 101).Select(i => new Point(0.01 * i, 0)).ToArray()));
            //lastTrainState = null;

            //ourCase2.Trainer.IterationFinished += (s, e) =>
            //{
            //    lastTrainState = e;
            //    statistic.Collect("Iteration", e);
            //    statistic.Collect("IterationCollect&TrainShow", e);
            //    statistic.ExecuteExtention<IShowExtention>("Iteration");
            //    Displayer.Instance.Display();
            //    //if (e.Iteration == 1)
            //    //    ourCase2.Trainer.BreakTrain();
            //};

            //Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            //{
            //    e.Cancel = true;
            //    ourCase2.Trainer.BreakTrain();
            //};

            //ourCase2.Trainer.Train(ourCase2.Steps);

            //if (lastTrainState != null)
            //{
            //    statistic.Collect("Train", lastTrainState);
            //    statistic.ExecuteExtention<IShowExtention>("Train");
            //    //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            //}
            //Console.ReadLine();

            //Func<Point, double> Solution =
            // p => p.Coordinate[0] * Math.Cos(p.Coordinate[1]);

            //var error = 0.0;
            //var exact = 0.0;

            //for (double x = -1; x <= 1; x += 0.1)
            //    for (double t = 0; t <= 10; t += 0.9)
            //    {
            //        var p = new Point(x, t);
            //        double pReal = ourCase.Network.Compute(p.Coordinate);
            //        double pExact = Solution(new Point(x, t));
            //        error += Math.Pow(pReal - pExact, 2);
            //        exact += Math.Pow(pExact, 2);
            //    }

            //Console.Write("!E: {0}", Math.Sqrt(error) / Math.Sqrt(exact));

            //Console.WriteLine("Next");


            //Case ourCase1 = KleinGordonPDE.Suite.Case_TR_2(ourCase.Network);
            //lastTrainState = null;

            //statistic = new PDE.Statistic.Statistic();
            //StatisticBuilder.Build(statistic, ourCase1);

            //ourCase1.Trainer.IterationFinished += (s, e) =>
            //                                          {
            //                                              e.Network = Build(ourCase.Network, e.Network);
            //                                              lastTrainState = e;
            //                                              statistic.Collect("Iteration", e);
            //                                              statistic.Collect("IterationCollect&TrainShow", e);
            //                                              statistic.ExecuteExtention<IShowExtention>("Iteration");
            //                                              Displayer.Instance.Display();
            //                                          };

            //Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            //                              {
            //                                  e.Cancel = true;
            //                                  ourCase1.Trainer.BreakTrain();
            //                              };

            //ourCase1.Trainer.Train(ourCase1.Steps);

            //if (lastTrainState != null)
            //{
            //    //lastTrainState.Network = ourCase1.Network;
            //    statistic.Collect("Train", lastTrainState);
            //    statistic.ExecuteExtention<IShowExtention>("Train");
            //    statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            //}
            //Console.WriteLine("Next");


            
            //error = 0.0;
            //exact = 0.0;

            //for(double x = -1; x <= 1; x+= 0.1)
            //    for(double t = 0; t <= 10; t+= 0.9)
            //    {
            //        var p = new Point(x, t);
            //        double pReal = ourCase.Network.Compute(p.Coordinate);
            //        if (x > -0.91 && x < 0.91 && t > 0.8)
            //            pReal += ourCase1.Network.Compute(p.Coordinate);
            //        double pExact = Solution(new Point(x, t));
            //        error += Math.Pow(pReal - pExact, 2);
            //        exact += Math.Pow(pExact, 2);
            //    }

            //Console.Write("!E: {0}", Math.Sqrt(error) / Math.Sqrt(exact));
            

            //var prev = Build(ourCase.Network, ourCase1.Network);
            //Case ourCase2 = KleinGordonPDE.Suite.Case_TR_3(prev);
            //lastTrainState = null;

            //statistic = new PDE.Statistic.Statistic();
            //StatisticBuilder.Build(statistic, ourCase2);

            //ourCase2.Trainer.IterationFinished += (s, e) =>
            //{
            //    e.Network = Build(prev, e.Network);
            //    lastTrainState = e;
            //    statistic.Collect("Iteration", e);
            //    statistic.Collect("IterationCollect&TrainShow", e);
            //    statistic.ExecuteExtention<IShowExtention>("Iteration");
            //    Displayer.Instance.Display();
            //};

            //Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            //{
            //    e.Cancel = true;
            //    ourCase2.Trainer.BreakTrain();
            //};

            //ourCase2.Trainer.Train(ourCase2.Steps);

            ////if (lastTrainState != null)
            ////{
            ////    //lastTrainState.Network = ourCase1.Network;
            ////    statistic.Collect("Train", lastTrainState);
            ////    statistic.ExecuteExtention<IShowExtention>("Train");
            ////    statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            ////}
            //Console.ReadLine();
            //var network = Build(ourCase.Network, ourCase1.Network);

            //var rand = new Random(357);
            //for (int i = 0; i < 10; i++)
            //{
            //    Case ourCase2 = KleinGordonPDE.Suite.Case_TR_2(network, (int)(rand.NextDouble() * 15) + 1, (int)(rand.NextDouble() * 10) + 1);
            //    lastTrainState = null;

            //    statistic = new PDE.Statistic.Statistic();
            //    StatisticBuilder.Build(statistic, ourCase2);

            //    ourCase2.Trainer.IterationFinished += (s, e) =>
            //                                              {
            //                                                  e.Network = Build(network, e.Network);
            //                                                  lastTrainState = e;
            //                                                  statistic.Collect("Iteration", e);
            //                                                  statistic.Collect("IterationCollect&TrainShow", e);
            //                                                  statistic.ExecuteExtention<IShowExtention>("Iteration");
            //                                                  Displayer.Instance.Display();
            //                                              };

            //    Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            //                                  {
            //                                      e.Cancel = true;
            //                                      ourCase2.Trainer.BreakTrain();
            //                                  };

            //    ourCase2.Trainer.Train(ourCase2.Steps);

            //    //if (lastTrainState != null)
            //    //{
            //    //    statistic.Collect("Train", lastTrainState);
            //    //    statistic.ExecuteExtention<IShowExtention>("Train");
            //    //    statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
            //    //}

            //    Console.ReadLine();

            //    network = Build(network, ourCase2.Network);
            //}
        }

      
        private static void IterationSolver()
        {
            Problem previous = null;
            for (int i = 0; i < 5; i++)
            {
                Case ourCase1 = KleinGordonPDE.Suite.Case_TR_1(i, previous);
                //ourCase1.Steps = 2;

                var statistic = new PDE.Statistic.Statistic();
                StatisticBuilder.Build(statistic, ourCase1);

                Trainer.TrainStateArg lastTrainState = null;

                ourCase1.Trainer.IterationFinished += (s, e) =>
                                                          {
                                                              lastTrainState = e;
                                                              statistic.Collect("Iteration", e);
                                                              statistic.Collect("IterationCollect&TrainShow", e);
                                                              statistic.ExecuteExtention<IShowExtention>("Iteration");
                                                              Displayer.Instance.Display();
                                                          };

                Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                                              {
                                                  e.Cancel = true;
                                                  ourCase1.Trainer.BreakTrain();
                                              };

                ourCase1.Trainer.Train(ourCase1.Steps);

                if (lastTrainState != null)
                {
                    statistic.Collect("Train", lastTrainState);
                    statistic.ExecuteExtention<IShowExtention>("Train");
                    //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
                }
                Console.ReadLine();
                previous = ourCase1.Problem;
            }
        }
    }
}
