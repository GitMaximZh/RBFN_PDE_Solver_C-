using System;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using OptimizationToolbox.Common;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;

namespace PDE.Function
{
    public class JacobianDifferentiableErrorFunction : IDifferentiableFunction
    {
        private readonly IControlPoint[] _points;
        private readonly Problem _problem;
        private readonly IErrorFunctional _errorFunctional;
        private readonly ProblemParameters _parametersToOptimize;

        public JacobianDifferentiableErrorFunction(Problem problem,
            IControlPoint[] points, IErrorFunctional errorFunctional, ProblemParameters parametersToOptimize)
        {
            _points = points;
            _problem = problem;
            _errorFunctional = errorFunctional;
            _parametersToOptimize = parametersToOptimize;
        }

        public DenseVector Gradient(Point point)
        {
            _problem.MoveToPoint(point, _parametersToOptimize);
            var diff = new DenseVector(_points.Length);

            for (int i = 0; i < _points.Length; i++)
                diff[i] = Math.Sqrt(_points[i].Weight) *
                    (_points[i].ApproximateValue() - _points[i].ExpectedValue());

            var jacobian = this.CalculateJacobian(_problem, _points, _parametersToOptimize);
            _problem.MoveBack(_parametersToOptimize);

            return 2 * new DenseVector(jacobian.Transpose() * diff);
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
