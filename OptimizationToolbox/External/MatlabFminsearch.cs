using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using MathWorks.MATLAB.NET.Arrays;
using OptimizationNative;
using OptimizationToolbox.Common;

namespace OptimizationToolbox.External
{
    public class  MFunction
    {
        private ITwiceDifferentiableFunction func;
        public MFunction(ITwiceDifferentiableFunction func)
        {
            this.func = func;
        }

        //public object evaluateFunction(double[] x)
        //{

        //    var res = new ob[2];

        //    var r0 = new MWNumericArray(1, 1, new[] { 5 });
        //    res[0] = r0;

        //    //var fg = func.Gradient(new Point(1));

        //    res[1] = new MWNumericArray(2, 1, 1);
        //    return res;
        //}

        public object evaluateFunction(double[] x)
        {

            var res = new object[3];
            var r0 = new double[1, 1];
            r0[0, 0] = func.Value(new Point(x));
            res[0] = r0;

            var fg = func.Gradient(new Point(x));
            var r1 = new double[x.Length, 1];
            for (int i = 0; i < x.Length; i++)
                r1[i, 0] = fg[i];
            res[1] = r1;
            res[2] = func.Hessian(new Point(x)).ToArray();
            return res;
        }
    }


    public class MatlabFminsearch : OptimizationMethod
    {
        private bool done = false;
        public MatlabFminsearch(ITwiceDifferentiableFunction function, Point startPoint)
            : base(function, startPoint)
        {
        }

        public override bool DoOptimizationStep()
        {
            if (done)
                return false;
            var optimizer = new Optimization();
            var result = optimizer.optimize(2, new MFunction((ITwiceDifferentiableFunction)Function), StartPoint.Coordinate);
            CurrentValue = ((double[,]) result[1])[0, 0];
            CurrentPoint = new Point(((double[,])result[0]).Cast<double>().ToArray());
            done = true;
            return true;
        }
    }
}
