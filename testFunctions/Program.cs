using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common;
using Plotting;

namespace testFunctions
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Spline(0, 1, new List<double>(new [] {1.0}));
            var points = new List<Point>();
            for(double x = -0.5; x < 1.5; x+= 0.01)
            {
                //for (double y = -1; y < 2; y += 0.1)
                {
                    points.Add(new Point(x, f.Value(x)));
                }
            }
            ShowGrapthic(points.ToArray());
            Console.ReadLine();
        }

        static void ShowGrapthic(Point[] points)
        {
            var dataPath = String.Format(@"{0}\{1}.dat", Environment.CurrentDirectory, "graphic_data");

            var graphic = new Graphic(new Function(points));
            graphic.Save(dataPath, true);

            var graphicArg = String.Format(@"{0}\{1}.gp", Environment.CurrentDirectory, "graphic");

            string program = @"D:\SoftwareDevelopment\gnuplot\bin\wgnuplot.exe";
            var extPro = new Process();
            extPro.StartInfo.FileName = program;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = false;
            extPro.StartInfo.Arguments = graphicArg +" -";
            extPro.Start();
        }
    }
}
