using System;
using Common;
using RBFNetwork.Train;

namespace PDE.Solution
{
    public class Case
    {
        public Problem Problem { get; private set; }
        public Trainer Trainer { get; private set; }
        public IPoint[] ControlPoints
        {
            get { return Trainer.ControlPoints; }
        }
    
        public IPoint[] GraphicPoints { get; set; }
        public IPoint[] CheckPoints { get; set; }
        public Func<IPoint, double> Solution { get; set; }
        public int Steps { get; set; }

        public Case(Problem problem, 
            Trainer trainer)
        {
            Problem = problem;
            Trainer = trainer;

            Steps = -1;
        }
    }
}
