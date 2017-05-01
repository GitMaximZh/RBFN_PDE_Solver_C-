using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace RBFNetwork.Function
{
    public class WendlandFunction : BasicFunction
    {
        public WendlandFunction(int dimensions)
            : base(dimensions, 1)
        {

        }

        public override double Calculate(double[] xs)
        {
            var a = Math.Pow(Parameters[0], 2);
            var x = xs[0];
            var cx = Center[0];
            var y = xs[1];
            var cy = Center[1];
            var d = 1.0/a;
            var r = Math.Sqrt((x - cx)*(x - cx) + (y - cy)*(y - cy));

            if (r >= d || r <= -d)
                return 0.0;

            return Math.Pow(1.0 - r * a, 3) * (1.0 + 3.0 * r * a) / 12.0;
                //Math.Pow(1.0 - r*a, 5)*(3.0 + 15.0*r*a + 24 * Math.Pow(r/a, 2))/840.0;
        }



        public override double FirstDerivation(double[] x, int dim)
        {
            return NumericTools.FirstOrderDerivation(p => Calculate(p.Coordinate), new Point(x), dim);
            //return 0.0;
        }

        public override double SecondDerivation(double[] x, int dim)
        {
            return NumericTools.SecondOrderDerivation(p => Calculate(p.Coordinate), new Point(x), dim);
            //return 0.0;
        }
    }
}
