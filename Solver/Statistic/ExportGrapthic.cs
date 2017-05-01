using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PDE.Statistic;
using Plotting;
using System.Globalization;

namespace Solver.Statistic
{
    internal class ExportGrapthic : Extention<GraphicCollector>, IShowExtention
    {
        public string DataFile { get; set; }

        public ExportGrapthic(GraphicCollector extendable)
            : base(extendable)
        {
        }

        public override void Execute()
        {
            var dataPath = String.Format(@"{0}\{1}.dat", Environment.CurrentDirectory, DataFile);

            var func = new Function(Extendable.Points.ToArray());
            StringBuilder content = new StringBuilder();
            AddPoints(content, func);

            FileStream fileStream = File.Open(dataPath,
                FileMode.Create,
                FileAccess.ReadWrite);
            fileStream.Seek(0, SeekOrigin.End);
            TextWriter tw = new StreamWriter(fileStream);

            if (fileStream.Length != 0)
            {
                tw.WriteLine();
                tw.WriteLine();
            }

            tw.Write(content.ToString());
            tw.Flush();
            tw.Close();

            fileStream.Close();
        }



        private void AddPoints(StringBuilder content, Function f)
        {
            double? prev = null;
            string prefix = "";
            foreach (var p in f.Points)
            {
                if (prev.HasValue && prev.Value != p.Coordinate[0])
                {
                    content.AppendLine();
                    prefix = "";
                }
                content.Append(prefix + p[2].ToString(CultureInfo.InvariantCulture));
                prefix = " ";
                prev = p.Coordinate[0];
            }
        }
    }
}
