using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Train;

namespace PDE.Solution
{
    public class ProblemFactory : IProblemFactory
    {
        private RBFNetworkSettings settings;
        private RBFNetworkFactory _networkFactory;

        public ProblemFactory(RBFNetworkSettings settings)
        {
            this.settings = settings;
            _networkFactory = new RBFNetworkFactory(settings);
        }

        public Problem Create()
        {
            return new Problem(_networkFactory.Create()) { PreviousNetwork = settings.Previous };
        }
    }
}
