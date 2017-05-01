using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using RBFNetwork.Function;

namespace RBFNetwork.Tools
{
    public class NetworkFactory
    {
        private readonly static Encoding ENCODING = Encoding.UTF8;

        private static NetworkFactory instance = new NetworkFactory();
        public static NetworkFactory Instance
        {
            get { return instance; }
        }

        private NetworkFactory()
        {
        }

        public RBFNetwork Create(string path)
        {
            if (!File.Exists(path))
                return null;
            var config = Deserialize(path);
            var network = CreateInstance(config);
            for (int i = 0; i < config.Functions.Count; i++)
            {
                var func = config.Functions[i];
                network.Weights[i].Value = func.Weight;
                for (int j = 0; j < func.Centers.Count; j++)
                    network.Functions[i].Center[j].Value = func.Centers[j];
                for (int j = 0; j < func.Parameters.Count; j++)
                    network.Functions[i].Parameters[j].Value = func.Parameters[j];
            }
            return network;
        }

        protected RBFNetwork CreateInstance(NetworkConfiguration config)
        {
            if(config.NetworkType == NetworksTypes.Base)
                return new RBFNetwork(config.InputsCount, config.FunctionsCount, GetRBFFunctionCreator(config)); 
            if(config.NetworkType == NetworksTypes.Normalized)
                new NormalizedRBFNetwork(config.InputsCount, config.FunctionsCount, GetRBFFunctionCreator(config));
            throw new ArgumentException("Wrong network type");
        }

        protected Func<IBasisFunction> GetRBFFunctionCreator(NetworkConfiguration config)
        {
            switch (config.FunctionsType)
            {
                case FunctionsTypes.Gaussian: 
                    return () => new GaussianFunction(config.InputsCount);
                case FunctionsTypes.Multiquadric:
                    return () => new MultiquadricFunction(config.InputsCount);
                case FunctionsTypes.Asymmetric:
                    return () => new AsymmetricGaussianFunction(config.InputsCount);
            }
            throw new ArgumentException("Wrong function type");
        }

        protected FunctionsTypes GetRBFFunctionType(RBFNetwork network)
        {
            if(network.Functions.Length == 0)
                throw new ArgumentException("Wrong function type");
            if (network.Functions[0].GetType().Equals(typeof(GaussianFunction)))
                return FunctionsTypes.Gaussian;
            if (network.Functions[0].GetType().Equals(typeof(MultiquadricFunction)))
                return FunctionsTypes.Multiquadric;
            if (network.Functions[0].GetType().Equals(typeof(AsymmetricGaussianFunction)))
                return FunctionsTypes.Asymmetric;
            throw new ArgumentException("Wrong function type");
        }

        public void Save(string path, RBFNetwork network)
        {
            var config = new NetworkConfiguration();
            config.InputsCount = network.InputCount;
            config.FunctionsCount = network.HiddenCount;
            config.NetworkType = network is NormalizedRBFNetwork ? NetworksTypes.Normalized : NetworksTypes.Base;

            config.FunctionsType = GetRBFFunctionType(network);
            config.Functions = new List<FunctionConfiguration>();

            for (int i = 0; i < network.HiddenCount; i++)
            {
                var funcConfig = new FunctionConfiguration();
                funcConfig.Weight = network.Weights[i];

                var func = network.Functions[i];

                funcConfig.Centers = new List<double>();
                for (int j = 0; j < func.Center.Length; j++)
                    funcConfig.Centers.Add(func.Center[j]);
                
                funcConfig.Parameters = new List<double>();
                for (int j = 0; j < func.Parameters.Length; j++)
                    funcConfig.Parameters.Add(func.Parameters[j]);

                config.Functions.Add(funcConfig);
            }

            Serialize(path, config);
        }

        private void Serialize(string path, NetworkConfiguration config)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetworkConfiguration));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(path, ENCODING);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlTextWriter, config);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't deserialze the job object. See an inner exeption", ex);
            }
        }

        private NetworkConfiguration Deserialize(string path)
        {
            try
            {
                var config = File.ReadAllText(path, ENCODING);
                MemoryStream memoryStream = new MemoryStream(ENCODING.GetBytes(config));
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetworkConfiguration));
                return (NetworkConfiguration)xmlSerializer.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't deserialze the job object. See an inner exeption", ex);
            }
        }
    }
}
