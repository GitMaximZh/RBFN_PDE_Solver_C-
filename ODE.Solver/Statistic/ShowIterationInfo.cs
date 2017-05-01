using System;
using PDE.Statistic;
using Solver.Statistic;

namespace ODE.Solver.Statistic
{
    internal class ShowIterationInfo : Extention<IterationInfoCollector>, IShowExtention
    {
        public ShowIterationInfo(IterationInfoCollector extendable) : base(extendable)
        {
        }

        public override void Execute()
        {
            Displayer.Instance.AddToDisplay("Iteration: " + Extendable.CurrentIteration);
            Displayer.Instance.AddToDisplay("Error: " + Extendable.CurrentError);
        }
    }
}
