﻿using System;
using System.Collections.Generic;
using Common;
using EITProblem.Model;
using MathNet.Numerics.LinearAlgebra.Double;
using PDE.Solution;
using RBFNetwork.Train;
using System.Linq;

namespace EITProblem.Direct
{
    internal class ControlPointsFactory : IControlPointsFactory
    {
        public int XDimention { get; set; }
        public int YDimention { get; set; }

        public Point ButtomLeftCorner { get; set; }
        public Point TopRightCorner { get; set; }

        private Model.Model model;
        public ControlPointsFactory(Model.Model model)
        {
            this.model = model;
        }

        public IControlPoint[] Create(Problem problem)
        {
            int index = 1;
            var points = GetBoundPoints(problem).Concat(GetInnerPoints(problem));
            points.ToList().ForEach(p => p.Index = index++);
            return points.ToArray();
        }

        protected virtual IControlPoint[] GetBoundPoints(Problem problem)
        {
            var points = new List<IControlPoint>();
            foreach (var side in model.Sides)
                foreach (var part in side.Parts)
                {
                    foreach (var point in part)
                    {
                        if (part is Electrode)
                        {
                            points.Add(new IElectrodeBoundControlPoint(problem, part.Weight, part.Normal,
                                                                       part.Value, part.Length/part.Density,
                                                                       point.Coordinate) {Tag = 2});
                            points.Add(new UElectrodeBoundControlPoint(problem, 10, (Electrode)part, point.Coordinate));
                        }
                        else
                            points.Add(new BoundControlPoint(problem, part.Weight, part.Normal,
                                                         part.Value, point.Coordinate) { Tag = 1 });
                        //if (part is Electrode)
                        //    points.Add(new SignControlPoint(problem, 20000, part.Value > 0, point.Coordinate) { Tag = 2 }); 
                    }
                }

            var e1 = model.Sides.SelectMany(s => s.Parts).OfType<Electrode>().First();
            var e2 = model.Sides.SelectMany(s => s.Parts).OfType<Electrode>().Last();

            //points.Add(new ElectrodeControlPoint(problem, 100, e1));
            //points.Add(new ElectrodeControlPoint(problem, 100, e2));
            //points.Add(new UElectrodeBoundControlPoint(problem, 1, e1));
            //points.Add(new UElectrodeBoundControlPoint(problem, 1, e2));
            points.Add(new IEqualityCheckPoint(problem, 5, e1, e2));
            points.Add(new UEqualityCheckPoint(problem, 10, e1, e2));
            //points.Add(new IEqualityCheckPoint(problem, 5, e1, e2));
            //points.Add(new UEqualityCheckPoint(problem, 5, e1, e2));
            
            return points.ToArray();
        }

        protected virtual IControlPoint[] GetInnerPoints(Problem problem)
        {
            var points = new List<IControlPoint>();

            var xstep = (TopRightCorner.Coordinate[0]
                          - ButtomLeftCorner.Coordinate[0]) / (XDimention - 1);
            var ystep = (TopRightCorner.Coordinate[1]
                          - ButtomLeftCorner.Coordinate[1]) / (YDimention - 1);

            for (int i = 0; i <= XDimention - 1; i++)
            {
                for (int j = 0; j <= YDimention - 1; j++)
                {
                    points.Add(new InnerControlPoint(problem, 1, ButtomLeftCorner.Coordinate[0] + xstep * i,
                                         ButtomLeftCorner.Coordinate[1] + ystep * j) { Tag = 0 });
                }
            }

            return points.ToArray();
        }
    }
}
