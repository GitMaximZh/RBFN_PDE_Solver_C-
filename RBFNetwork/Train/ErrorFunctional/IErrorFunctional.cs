using System;
using Common;

namespace RBFNetwork.Train.ErrorFunctional
{
    public interface IErrorFunctional
    {
        double Error(IControlPoint[] points);
    }
}
