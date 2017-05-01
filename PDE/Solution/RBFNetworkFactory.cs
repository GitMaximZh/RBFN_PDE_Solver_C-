using System;
using System.Collections.Generic;
using System.Linq;

namespace PDE.Solution
{
    public class RBFNetworkFactory : IRBFNetworkFactory
    {
        private static readonly Random Rand = new Random(51523);

        private readonly RBFNetworkSettings _settings;

        public RBFNetworkFactory(RBFNetworkSettings settings)
        {
            _settings = settings;
            _settings.Validate();
        }

        public RBFNetwork.RBFNetwork Create()
        {
            var network = _settings.NetworkType == NetworkType.Base ? 
                new RBFNetwork.RBFNetwork(_settings.Dimention, GetFunctionsCount(), _settings.FunctionCreator) :
                new RBFNetwork.NormalizedRBFNetwork(_settings.Dimention, GetFunctionsCount(), _settings.FunctionCreator);
            BuildNetwork(network);
            return network;
        }

        private void BuildNetwork(RBFNetwork.RBFNetwork network)
        {
            //-----------Parameters-------------
            for (int i = 0; i < _settings.ParametersSettings.Count; i++)
            {
                var setting = _settings.ParametersSettings[i];
                Func<double> getParameter;
                if (setting.deviation.HasValue)
                    getParameter =
                        () => (setting.value - setting.deviation.Value) + Rand.NextDouble() * 2 * setting.deviation.Value;
                else
                    getParameter = () => setting.value;

                foreach (var f in network.Functions)
                    f.Parameters[i].Value = getParameter();
            }


            //-----------Centers----------
            var lengthes = new List<double>();
            for (int i = 0; i < _settings.Dimention; i++)
                lengthes.Add(_settings.EndPoint.Coordinate[i] - _settings.StartPoint.Coordinate[i]);

            if (_settings.FunctionsCount.HasValue)
            {
                foreach (var f in network.Functions)
                {
                    for (int i = 0; i < _settings.Dimention; i++)
                        f.Center[i].Value = _settings.StartPoint.Coordinate[i] + lengthes[i] * Rand.NextDouble();
                }
            }
            else
            {
                var steps = lengthes.Select((l, i) => l / (_settings.CenterXDimentions[i] - 1)).ToList();
                for (int j = 0; j < network.Functions.Length; j++)
                {
                    var index = j;
                    var dim = 1;
                    for (int i = 0; i < _settings.Dimention; i++)
                    {
                        var nDim = dim*_settings.CenterXDimentions[i];
                        var correction = index % nDim;
                        index -= correction;
                        network.Functions[j].Center[i].Value =
                            _settings.StartPoint.Coordinate[i] + steps[i] * correction / dim;
                        dim = nDim;
                    }
                }
            }

            //-----------Weight-----------
            Func<double> getWeight;
            if (_settings.WeightSettings.deviation.HasValue)
                getWeight = () => (_settings.WeightSettings.value - _settings.WeightSettings.deviation.Value) +
                    Rand.NextDouble() * 2 * _settings.WeightSettings.deviation.Value;
            else
                getWeight = () => _settings.WeightSettings.value;

            foreach (var w in network.Weights)
                w.Value = getWeight();
        }

        private int GetFunctionsCount()
        {
            if (_settings.FunctionsCount.HasValue)
                return _settings.FunctionsCount.Value;
            var fc = 1;
            _settings.CenterXDimentions.ForEach(v => fc *= v);
            return fc;
        }
    }
}
