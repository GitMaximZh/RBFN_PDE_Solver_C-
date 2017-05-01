using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testFunctions
{
    public class BSpline
    {
        public double C { get; private set; }
        public double A { get; private set; }
        public double Phi { get; private set; }
        public double Lambda { get; private set; }

        private double f;
        private double g;
        private double a;

        public BSpline(double c, double a, double lambda, double phi)
        {
            C = c;
            A = a;
            Phi = phi;
            Lambda = lambda;

            this.a = A;
            this.f = phi;
            g = lambda;
            //this.a = Math.Exp(A);
            //g = Scale(Lambda);
            //f = Scale(phi);
        }

        public double Calculate(double x)
        {
            var a = 1.0;
            var h = a / 3.0;

            var res = 0.0;
            if (x > C + a)
                return res;

            res += 1.0 / (2.0 * Math.Pow(h, 3)) * Math.Pow(C + a - x, 2);

            if (x > C + 2 * h)
                return res;
            res += -3.0 / (2.0 * Math.Pow(h, 3)) * Math.Pow(C + 2 * h - x, 2);

            if (x > C + h)
                return res;
            res += 3.0 / (2.0 * Math.Pow(h, 3)) * Math.Pow(C + h - x, 2);

            if (x > C)
                return res;
            res += -1.0 / (2.0 * Math.Pow(h, 3)) * Math.Pow(C - x, 2);
            return res;
        }

        public double Value(double x)
        {
            var res = 0.0;
            if (x > C + a)
                return res;

            res += Math.Pow(C + a - x, 2) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += Math.Pow(C + a * f - x, 2) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f *g)
                return res;
            res += Math.Pow(C + a * f * g - x, 2) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += Math.Pow(C - x, 2) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);
            return res;
        }

        public double I1Value(double x)
        {
            var C1 = 2.0;
            var res = 0.0;
            if (x > C + a)
                return res;

            res += -1.0 /3.0 * Math.Pow(C + a - x, 3) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C + a * f - x, 3) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f * g)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C + a * f * g - x, 3) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += -1.0 / 3.0 * Math.Pow(C - x, 3) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);
            return res;
        }

        public double I2Value(double x)
        {
            var res = 0.0;
            if (x > C + a)
                return res;

            res += 1.0 / 12.0 * Math.Pow(C + a - x, 4) / (Math.Pow(a, 3) * (1 - f * g) * (1 - f));

            if (x > C + a * f)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C + a * f - x, 4) / (Math.Pow(f, 2) * Math.Pow(a, 3) * (1 - g) * (f - 1));

            if (x > C + a * f * g)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C + a * f * g - x, 4) / (Math.Pow(f, 2) * Math.Pow(a, 3) * g * (g - 1) * (f * g - 1));

            if (x > C)
                return res;
            res += 1.0 / 12.0 * Math.Pow(C - x, 4) / (-Math.Pow(f, 2) * Math.Pow(a, 3) * g);
            return res;
        }

        private double Scale(double x)
        {
            return 1.0 / Math.PI * Math.Atan(x) + 0.5; // Math.Exp(x) / (1.0 + Math.Exp(x));
        }
    }
}
