using System;
using System.Linq;
using Common;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using RBFNetwork;
using RBFNetwork.Train;
using RBFNetwork.Train.ErrorFunctional;
using MathNet.Numerics.LinearAlgebra.Double;

namespace PDE.Optimizer
{
    public class SVDOptimizer : IOptimizer
    {
        public static Svd svd;
        public static bool useOld = false;

        private readonly ProblemParameters parameters = new ProblemParameters { NetworkParameters = new NetworkParameters(true, false, false) };
        public bool ShouldBeStoped(int step, double error, double previousError)
        {
            return step == 1;
        }

        public double Optimize(Problem problem, IErrorFunctional functional,
            IControlPoint[] controlPoints, int step, double error, bool optimizationProblemChanged)
        {
            controlPoints = controlPoints.ToArray();
            
            var network = problem.Network;
            var functionCount = network.HiddenCount;
            var pointsCount = controlPoints.Length;
            
            var A = new DenseMatrix(pointsCount, functionCount);
            var b = new DenseVector(pointsCount);
            if (!useOld)
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    var point = controlPoints[i];
                    for (int j = 0; j < functionCount; j++)
                    {
                        //throw  new Exception();
                        A[i, j] = point.ApproximateValue(network.Functions[j]);
                    }
                    b[i] = point.ExpectedValue();
                }
                
                svd = A.Svd(true);
            }
            else
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    var point = controlPoints[i];
                    b[i] = point.ExpectedValue();
                }
            }



            var c = svd.U().Transpose() * b;

            var y = new DenseVector(functionCount);
            c.SubVector(0, svd.Rank).PointwiseDivide(svd.S().SubVector(0, svd.Rank))
                .CopySubVectorTo(y, 0, 0, svd.Rank);
            
            var x = svd.VT().Transpose() * y;

            network.MoveToPoint(new Point(x.ToArray()), parameters.NetworkParameters);
            return functional.Error(controlPoints);
        }
    }
}
