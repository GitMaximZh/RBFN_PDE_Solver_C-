using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODE.Solver.Statistic;
using PDE.Optimizer;
using PDE.Solution;
using RBFNetwork.Train;

namespace ODE.Solver
{
    class Program
    {
        private static void Solver()
        {
            Case ourCase = InverseProblem1.Suite.Case_1();

            var statistic = new PDE.Statistic.Statistic();
            StatisticBuilder.Build(statistic, ourCase);

            Trainer.TrainStateArg lastTrainState = null;

            ourCase.Trainer.IterationFinished += (s, e) =>
            {
                lastTrainState = e;
                statistic.Collect("Iteration", e);
                statistic.ExecuteExtention<IShowExtention>("Iteration");
                Displayer.Instance.Display();
            };

            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                ourCase.Trainer.BreakTrain();
            };

            ourCase.Trainer.Train(ourCase.Steps);

            if (lastTrainState != null)
            {
                statistic.Collect("Train", lastTrainState);
                statistic.ExecuteExtention<IShowExtention>("Train");
            }
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            dTSolver();
        }

        private static void dTSolver()
        {
            Problem un = null;
            Problem un_1 = null;
            var dt = 0.03;
            for (double t = 0 + dt; t < 10 + dt; t += dt)
            {
                Case ourCase1 = KleinGordonPDE.Discrete.Suite.Case_TR(t, dt, un, un_1);


                var statistic = new PDE.Statistic.Statistic();
                StatisticBuilder.Build(statistic, ourCase1);

                Trainer.TrainStateArg lastTrainState = null;

                Console.WriteLine("t={0}", t);
                ourCase1.Trainer.IterationFinished += (s, e) =>
                {
                    lastTrainState = e;
                    statistic.Collect("Iteration", e);
                    //statistic.Collect("IterationCollect&TrainShow", e);
                    statistic.ExecuteExtention<IShowExtention>("Iteration");
                    Displayer.Instance.Display();
                    //if(e.Iteration == 5)
                    //    ourCase1.Trainer.BreakTrain();
                };

                Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    ourCase1.Trainer.BreakTrain();

                };

                ourCase1.Trainer.Train(ourCase1.Steps);

                if (lastTrainState != null)
                {
                    //statistic.Collect("IterationCollect&TrainShow", e);
                    //Console.Write("T={0}", t);
                    //statistic.Collect("Train", lastTrainState);
                    //statistic.ExecuteExtention<IShowExtention>("Train");
                    //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
                }
                //if ((int)t == t)
                //  Console.ReadLine();
                un_1 = un;
                un = ourCase1.Problem;
            }
            Console.ReadLine();
        }

        //klein gordon svd 
        //private static void dTSolver()
        //{
        //    Problem un = null;
        //    Problem un_1 = null;
        //    var dt = 0.03;
        //    // for (double t = 0 + dt, a = 0.005; a < 0.006; a += 0.000002)
        //    //for (double t = 0 + dt, a = 0.001; a < 0.1; a += 0.0005)
        //    int step = 1;
        //    for (double t = 0 + dt; t < 10 + dt; t += dt)
        //    {
        //        if (step == 3)
        //            SVDOptimizer.useOld = true;
        //        Case ourCase1 = KleinGordonPDE.Discrete.Suite.Case_SVD(t, dt, un, un_1, 0.07);
                

        //        var statistic = new PDE.Statistic.Statistic();
        //        StatisticBuilder.Build(statistic, ourCase1);

        //        Trainer.TrainStateArg lastTrainState = null;

        //        Console.WriteLine("t={0}", t);
        //        ourCase1.Trainer.IterationFinished += (s, e) =>
        //        {
        //            lastTrainState = e;
        //            statistic.Collect("Iteration", e);
        //            //statistic.Collect("IterationCollect&TrainShow", e);
        //            statistic.ExecuteExtention<IShowExtention>("Iteration");
        //            Displayer.Instance.Display();
        //            //if(e.Iteration == 5)
        //            //    ourCase1.Trainer.BreakTrain();
        //        };

        //        Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
        //        {
        //            e.Cancel = true;
        //            ourCase1.Trainer.BreakTrain();

        //        };

        //        ourCase1.Trainer.Train(ourCase1.Steps);

        //        if (lastTrainState != null)
        //        {
        //            //statistic.Collect("IterationCollect&TrainShow", e);
        //            //Console.Write("T={0}", t);
        //            //statistic.Collect("Train", lastTrainState);
        //            //statistic.ExecuteExtention<IShowExtention>("Train");
        //            //statistic.ExecuteExtention<IShowExtention>("IterationCollect&TrainShow");
        //        }
        //        //if ((int)t == t)
        //        //  Console.ReadLine();
        //        un_1 = un;
        //        un = ourCase1.Problem;
        //        step++;
        //    }
        //    Console.ReadLine();
        //}

        
    }
}
