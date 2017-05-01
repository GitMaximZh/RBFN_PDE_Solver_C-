using System;
using Common;

namespace RBFNetwork.Train.ErrorFunctional
{
    public class QuadraticErrorFunctional : IErrorFunctional
    {
        private int currentPassId = 0;
        public double Error(IControlPoint[] points)
        {
            currentPassId++;

            var error = 0.0;
            foreach (var controlPoint in points)
            {
                controlPoint.CurrentPassId = currentPassId;
                error += controlPoint.Weight * Math.Pow(controlPoint.ApproximateValue() 
                    - controlPoint.ExpectedValue(), 2);
            }
            return error;
        }
    }
}
