using System.Collections.Generic;
using System;
using RBFNetwork.Train.ErrorFunctional;

namespace RBFNetwork.Train
{
    public class Trainer
    {
        public class TrainStateArg : EventArgs
        {
            public Problem Problem { get; set; }
            public IControlPoint[] ControlPoints { get; set; }

            public double Error { get; set; }
            public int Iteration { get; set; }

            public IList<ErrorInfo> ErrorsHistory { get; set; }

            public bool IsLastIteration { get; set; }
        }

        public struct ErrorInfo
        {
            public int Iteration;
            public double Error;
        }

        public Problem Problem { get; private set; }
        public IControlPoint[] ControlPoints { get; private set; }
        public IErrorFunctional ErrorFunctional { get; private set; }

        public double CurrentError { get; private set; }
        public int CurrentStep { get; private set; }

        public IList<ErrorInfo> ErrorsHistory { get; private set; }

        private bool _breakTrain;

        public IOptimizer Optimizer { get; set; }
        public IControlPointsTransformer ContolPointsTransformer { get; set; }

        public event EventHandler<TrainStateArg> IterationFinished;

        public Trainer(Problem problem,
            IErrorFunctional functional,
            IControlPoint[] controlPoints)
        {
            Problem = problem;
            ErrorFunctional = functional;
            ControlPoints = controlPoints;
        }

        public virtual void Train(int steps = -1)
        {
            Initialize();

            CurrentError = ErrorFunctional.Error(ControlPoints);
            CurrentStep = 0;
            AddToErrorHistory(CurrentError, CurrentStep);

            if (Optimizer == null)
                return;
        
            while (!_breakTrain && CurrentStep != steps)
            {
                var optimizationProblemChanged = false;
                if(ContolPointsTransformer != null && 
                    ContolPointsTransformer.ShouldBeTransformed(CurrentStep))
                {
                    ControlPoints = ContolPointsTransformer.Transform(ControlPoints);
                    optimizationProblemChanged = true;

                    CurrentError = ErrorFunctional.Error(ControlPoints);
                    AddToErrorHistory(CurrentError, CurrentStep);
                }
                
                CurrentStep++;
                var previousError = CurrentError;
                CurrentError = Optimizer.Optimize(Problem, ErrorFunctional, 
                    ControlPoints, CurrentStep, previousError, optimizationProblemChanged);
                AddToErrorHistory(CurrentError, CurrentStep);

                bool stopTrain = Optimizer.ShouldBeStoped(CurrentStep, CurrentError, previousError);
                OnIterationFinished(stopTrain);

                if (stopTrain)
                    break;
            }
        }

        protected virtual void OnIterationFinished(bool isLastIteration)
        {
            if(IterationFinished != null)
                IterationFinished(this, new TrainStateArg
                                            {
                                                Iteration = CurrentStep,
                                                Error = CurrentError,
                                                ControlPoints = ControlPoints,
                                                Problem = Problem,
                                                ErrorsHistory = ErrorsHistory,
                                                IsLastIteration = isLastIteration
                                            });
        }

        protected virtual void Initialize()
        {
            ErrorsHistory = new List<ErrorInfo>();
        }

        public void BreakTrain()
        {
            _breakTrain = true;
        }

        private void AddToErrorHistory(double error, int step)
        {
            ErrorsHistory.Add(new ErrorInfo { Error = error, Iteration = step });
        }
    }
}
