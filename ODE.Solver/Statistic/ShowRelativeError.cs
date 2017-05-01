using System;
using PDE.Statistic;

namespace ODE.Solver.Statistic
{
    internal class ShowRelativeError : Extention<RelativeErrorCollector>, IShowExtention
    {
        public ShowRelativeError(RelativeErrorCollector extendable) : base(extendable)
        {
        }

        public override void Execute()
        {
            Displayer.Instance.AddToDisplay("Relative error: " + Extendable.RelativeError);
        }
    }
}
