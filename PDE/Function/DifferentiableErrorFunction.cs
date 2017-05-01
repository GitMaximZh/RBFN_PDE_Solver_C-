using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using OptimizationToolbox.Common;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Function
{
    public class DifferentiableErrorFunction : IDifferentiableFunction
    {
        private readonly IControlPoint[] _points;
        private readonly Problem _problem;
        private readonly IErrorFunctional _errorFunctional;
        private readonly ProblemParameters _parametersToOptimize;

        public DifferentiableErrorFunction(Problem problem,
            IControlPoint[] points, IErrorFunctional errorFunctional, ProblemParameters parametersToOptimize)
        {
            _points = points;
            _problem = problem;
            _errorFunctional = errorFunctional;
            _parametersToOptimize = parametersToOptimize;
        }

        public DenseVector Gradient(Point point)
        {
            var h = NumericTools.h;
            _problem.MoveToPoint(point, _parametersToOptimize);
            var p0 = _errorFunctional.Error(_points); 

            var gradient = new DenseVector(point.Dimension);
            for (int i = 0; i < point.Dimension; i++)
            {
                var parameter = _problem.GetParameterByCoordinate(i, _parametersToOptimize);
                parameter.Value += h;
                gradient[i] = (_errorFunctional.Error(_points) - p0) / h;
                parameter.Undo();
            }

            _problem.MoveBack(_parametersToOptimize);
            return gradient;
        }

        public double Value(Point point)
        {
            _problem.MoveToPoint(point, _parametersToOptimize);
            var error = _errorFunctional.Error(_points);
            _problem.MoveBack(_parametersToOptimize);
            return error;
        }
    }
}
