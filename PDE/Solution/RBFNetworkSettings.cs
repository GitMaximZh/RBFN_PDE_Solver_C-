using System;
using System.Collections.Generic;
using Common;
using RBFNetwork.Function;

namespace PDE.Solution
{
    public struct ParameterSettings
    {
        public double value;
        public double? deviation;
    }

    public enum NetworkType
    {
        Base,
        Normalized
    }

    public class RBFNetworkSettings : ICloneable
    {
        public NetworkType NetworkType { get; set; }
        public int Dimention { get; set; }
        public int? FunctionsCount { get; set; }
        public Func<IBasisFunction> FunctionCreator { get; set; }

        public List<ParameterSettings> ParametersSettings { get; set; }

        public ParameterSettings WeightSettings { get; set; }

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public List<int> CenterXDimentions { get; set; }

        public RBFNetwork.RBFNetwork Previous { get; set; }

        public void Validate()
        {
            Dimention.RequiredThat("Dimention").NotEmpty();
            FunctionCreator.RequiredThat("FunctionCreator").NotNull();

            StartPoint.RequiredThat("StartPoint").NotNull();
            EndPoint.RequiredThat("EndPoint").NotNull();

            if (!FunctionsCount.HasValue)
            {
                CenterXDimentions.RequiredThat("CentersXDimention").NotNull();
                if(Dimention != CenterXDimentions.Count)
                    throw new ArgumentException("CentersXDimention");
            }
        }

        public object Clone()
        {
            var clone = (RBFNetworkSettings)this.MemberwiseClone();
            clone.ParametersSettings = new List<ParameterSettings>(this.ParametersSettings);
            return clone;
        }
    }
}
