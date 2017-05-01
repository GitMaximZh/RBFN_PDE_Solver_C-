using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RBFNetwork.Tools
{
    public class FunctionConfiguration
    {
        public double Weight { get; set; }

        [XmlArrayItem(ElementName = "Center")]
        public List<double> Centers { get; set; }

        [XmlArrayItem(ElementName = "Parameter")]
        public List<double> Parameters { get; set; }
    }

    public enum NetworksTypes { Base, Normalized }
    public enum FunctionsTypes { Gaussian, Multiquadric, Asymmetric }

    [XmlRoot(ElementName = "Network")]
    public class NetworkConfiguration
    {
        public NetworksTypes NetworkType { get; set; }

        public int InputsCount { get; set; }

        public int FunctionsCount { get; set; }

        public FunctionsTypes FunctionsType { get; set; }

        [XmlArrayItem(ElementName = "Function")]
        public List<FunctionConfiguration> Functions { get; set; }
    }
}
