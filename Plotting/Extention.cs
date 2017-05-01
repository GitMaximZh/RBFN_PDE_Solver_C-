using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace Plotting
{
    public static class Extention
    {
        public static bool Save(this Graphic grapthic, string file, bool rewrite = false)
        {
            StringBuilder content = new StringBuilder();
            AddPoints(content, grapthic);

            FileStream fileStream = File.Open(file,
                rewrite ? FileMode.Create : FileMode.OpenOrCreate,
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
            return true;
        }

        private static void AddPoints(StringBuilder content, Graphic grapthic)
        {
            if (grapthic == null || grapthic.Functions == null || grapthic.Functions.Length == 0)
                return;
            
            int index = 0;
            while (true)
            {
                var mainFunc = grapthic.Functions[0];
                if (mainFunc.Points.Length <= index)
                    break;

                if (index > 0 && mainFunc.Points[0].Dimension == 3 &&
                    mainFunc.Points[index].Coordinate[0] != mainFunc.Points[index - 1].Coordinate[0])
                    content.AppendLine();

                string prefix = "";
                foreach (var f in grapthic.Functions)
                {
                    content.Append(prefix + String.Join(" ",
                        f.Points[index].Coordinate.Select(s => s.ToString(CultureInfo.InvariantCulture))));
                    prefix = " ";
                }
                content.Append(Environment.NewLine);

                index++;
            }
        }
    }
}
