using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RBFNetwork.Tools;
using RBFNetwork.Train;

namespace PDE.Solution
{
    public class FromFileProblemFactory : IProblemFactory
    {
        private string path;

        public FromFileProblemFactory(string pathToConfig)
        {
            path = pathToConfig;
        }

        public Problem Create()
        {
            return new Problem(NetworkFactory.Instance.Create(path));
        }
    }
}
