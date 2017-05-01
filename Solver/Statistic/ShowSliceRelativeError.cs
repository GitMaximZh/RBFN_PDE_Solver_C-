using System;
using PDE.Statistic;

namespace Solver.Statistic
{
    internal class ShowCoefficientRelativeError : Extention<CoefficientRelativeErrorCollector>, IShowExtention
    {
        public ShowCoefficientRelativeError(CoefficientRelativeErrorCollector extendable)
            : base(extendable)
        {
        }

        public override void Execute()
        {
            Displayer.Instance.AddToDisplay("Coefficient error: " + Extendable.RelativeError);
        }
    }
}
