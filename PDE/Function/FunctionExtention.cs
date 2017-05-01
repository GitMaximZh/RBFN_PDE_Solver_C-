using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using OptimizationToolbox.Common;
using RBFNetwork.Train;

namespace PDE.Function
{
    public static class FunctionExtention
    {
        public static DenseMatrix CalculateJacobian(this IFunction function, Problem problem,
            IControlPoint[] points, ProblemParameters parametersToOptimize)
        {
            var parameters = problem.GetOptimizedParameters(parametersToOptimize).ToList();
            var J = new DenseMatrix(points.Length, parameters.Count);
            
            var h = NumericTools.h;

            for (int i = 0; i < points.Length; i++)
            {
                var point = points[i];
                for (int j = 0; j < parameters.Count; j++)
                {
                    var p0 = Math.Sqrt(point.Weight) * point.ApproximateValue();

                    parameters[j].Value += h;
                    double p1 = Math.Sqrt(point.Weight) * point.ApproximateValue();
                    J[i, j] = (p1 - p0) / h;
                    parameters[j].Undo();
                }
            }

            return J;
        }
    }
}
