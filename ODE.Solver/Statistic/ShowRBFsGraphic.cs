using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PDE.Statistic;
using Plotting;
using Solver.Statistic;

namespace ODE.Solver.Statistic
{
    internal class ShowRBFsGraphic : ShowGraphic
    {
        public ShowRBFsGraphic(RBFsGraphicCollector extendable)
            : base(extendable)
        {
        }

        public override void Execute()
        {
            var graphicArg = String.Format(@"{0}\{1}.gp", Environment.CurrentDirectory, GrapthicScript);
            if (!File.Exists(graphicArg))
                return;

            var lines = File.ReadAllLines(graphicArg);
            if(lines.Length == 0)
                return;

            lines[0] = "n=" + Extendable.Points.First().Dimension;
            File.WriteAllLines(graphicArg, lines);

            base.Execute();
        }
    }
}
