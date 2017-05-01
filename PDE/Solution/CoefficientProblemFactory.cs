using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Train;

namespace PDE.Solution
{
    public class CoefficientProblemFactory : IProblemFactory
    {
        private RBFNetwork.RBFNetwork _network;
        private RBFNetwork.RBFNetwork _approximator;

        public CoefficientProblemFactory(RBFNetwork.RBFNetwork network, RBFNetwork.RBFNetwork coefficientNetwork)
        {
            _network = network;
            _approximator = coefficientNetwork;
        }

        public CoefficientProblemFactory(RBFNetworkSettings nSettings, RBFNetworkSettings aSettings)
            : this(new RBFNetworkFactory(nSettings).Create(), new RBFNetworkFactory(aSettings).Create()) { }
        
        public CoefficientProblemFactory(RBFNetwork.RBFNetwork network, RBFNetworkSettings aSettings)
            : this(network, new RBFNetworkFactory(aSettings).Create()) { }
       

        public CoefficientProblemFactory(RBFNetworkSettings nSettings, RBFNetwork.RBFNetwork coefficientNetwork)
               : this(new RBFNetworkFactory(nSettings).Create(), coefficientNetwork) { }
        
        public Problem Create()
        {
            return new CoefficientProblem(_network, _approximator);
        }
    }
}
