using System;
using MathWorks.MATLAB.NET.Arrays;
using testtr;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            MWObjectArray objFunc = null;		 // .NET objective function instance passed to component 
            MWNumericArray initPoints = null; // Initial points for optimization 
            MWNumericArray localMin = null;	 // Location of minimum value
            MWNumericArray minFuncVal = null; // Minimum function value 
            MWArray[] result = { null, null };	 // Result array

            var optimizer = new Optimization.Optimization();
            initPoints = new MWNumericArray(new double[] { -1.2, 1.0 });

            OFunc objectiveFunc = new OFunc();
            objFunc = new MWObjectArray(objectiveFunc);
            result = optimizer.optimize(2, objFunc, initPoints);

            localMin = (MWNumericArray)result[0];
            minFuncVal = (MWNumericArray)result[1];

            Console.WriteLine("Location of minimum: {0}", localMin);
            Console.WriteLine("Function value at minimum: {0}", minFuncVal);

            Console.ReadLine(); 
        }
    }
}
