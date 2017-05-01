using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDE.Statistic;

namespace Solver.Statistic
{
    internal class ShowControlPointsError : Extention<ControlPointsErrorCollector>, IShowExtention
    {
        public ShowControlPointsError(ControlPointsErrorCollector extendable)
            : base(extendable)
        {
        }

        public override void Execute()
        {
            Displayer.Instance.AddToDisplay("Residual: " + Extendable.Error);
        }
    }
}
