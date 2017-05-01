using System;
using System.Diagnostics;
using System.Linq;
using PDE.Statistic;
using Plotting;

namespace Solver.Statistic
{
    internal class ShowGraphic : Extention<GraphicCollector>, IShowExtention
    {
        public string ConfigurationScript { get; set; }
        public string GrapthicScript { get; set; }

        public string DataFile { get; set; }

        public ShowGraphic(GraphicCollector extendable) : base(extendable)
        {
        }

        public override void Execute()
        {
            var dataPath = String.Format(@"{0}\{1}.dat", Environment.CurrentDirectory, DataFile);

            var graphic = new Graphic(new Function(Extendable.Points.ToArray()));
            graphic.Save(dataPath, true);

            var configArg = "";
            if(!string.IsNullOrEmpty(ConfigurationScript))
                configArg = String.Format(@"{0}\{1}.gp", Environment.CurrentDirectory, ConfigurationScript) + " ";
            var graphicArg = String.Format(@"{0}\{1}.gp", Environment.CurrentDirectory, GrapthicScript);

            string program = @"D:\SoftwareDevelopment\gnuplot\bin\wgnuplot.exe";
            var extPro = new Process();
            extPro.StartInfo.FileName = program;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = false;
            extPro.StartInfo.Arguments = configArg + graphicArg + " -";
            extPro.Start();
        }
    }
}
