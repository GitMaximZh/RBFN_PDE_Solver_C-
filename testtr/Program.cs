using System;
using System.Linq;
using MathWorks.MATLAB.NET.Arrays;
using OptimizationToolbox.External;

namespace testtr
{
    class Program
    {
        static void Main(string[] args)
        {
            //var f = new Function();
            //var solver = new QuickGradientMethod(f, new Point(1.6, 0.6));
            //solver.SpeedFinder = new GoldenSectionSpeedFinder();
            //solver.Speed = 0.01;
            ////solver.TrustRegion = 2.0;
            ////solver.Mu1 = 0.2;
            ////solver.Mu2 = 0.7;
            //while (solver.DoOptimizationStep())
            //{
            //    Console.WriteLine(solver.CurrentValue + "   " + solver.CurrentPoint[0] + ":" + solver.CurrentPoint[1]);
            //}

            ////var m = new DenseMatrix(3, 3);
            ////for (int i = 0; i < 9; i++)
            ////{
            ////    m[i/3, i%3] = i;
            ////}
            //Console.ReadLine();
            
            
            //object[] result = { null, null };	 // Result array

           // var optimizer = new Optimization.Optimization();
           // optimizer.displayObj(new MWObjectArray(new MFunction(null)));
       
            //OFunc objectiveFunc = new OFunc();


            //result = optimizer.optimize(2, objectiveFunc, new double[] { -1.2, 1.0 });

            //var localMin = ((double[,])result[0]).Cast<double>().ToList();
            //var minFuncVal = result[1];

            //Console.WriteLine("Location of minimum: {0}", localMin);
            //Console.WriteLine("Function value at minimum: {0}", minFuncVal);

            Console.ReadLine(); 
        }
    }
}
