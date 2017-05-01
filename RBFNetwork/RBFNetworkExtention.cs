using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using RBFNetwork.Common;

namespace RBFNetwork
{
    public class NetworkParameters
    {
        public bool UseWeights { get; set; }
        public bool UseCenters { get; set; }

        private bool useAllParameters;

        public bool UseAllParmeters
        {
            get { return useAllParameters && indexes.Count == 0; }
            set
            {
                useAllParameters = value;
                if (useAllParameters)
                    indexes.Clear();
            }
        }

        private List<int> indexes = new List<int>(); 
        public int[] Indexes { get { return indexes.ToArray(); } }

        public NetworkParameters(bool useWeights, bool useCenters, params int[] indexes)
        {
            UseWeights = useWeights;
            UseCenters = useCenters;
            this.indexes.AddRange(indexes.Distinct());
        }

        public NetworkParameters(bool useWeights, bool useCenters, bool useAllParameters)
        {
            UseWeights = useWeights;
            UseCenters = useCenters;
            UseAllParmeters = useAllParameters;
        }

        public bool AddIndex(int index)
        {
            if (indexes.Contains(index))
                return false;
            indexes.Add(index);
            return true;
        }

        public bool AddIndexes(params int[] sequence)
        {
            var except = sequence.Except(indexes);
            if (!except.Any())
                return false;
            indexes.AddRange(except);
            return true;
        }

        public bool AddIndexesRange(int from, int to)
        {
            if (to < from)
                return false;
            if (to == from)
                return AddIndex(from);
            return AddIndexes(Enumerable.Range(from, to - from).ToArray());
        }
    }

    public static class RBFNetworkExtention
    {
        public static int GetParametersCount(this RBFNetwork network, NetworkParameters parameters)
        {
            var res = 0;
            if (parameters.UseWeights)
                res += network.HiddenCount;
            if (parameters.UseCenters)
            {
                for (int i = 0; i < network.InputCount; i++)
                    res += network.HiddenCount;
            }
            var indexes = parameters.UseAllParmeters ? Enumerable.Range(0, network.Functions[0].Parameters.Length) : parameters.Indexes;
            if (indexes.Any())
            {
                foreach (var index in indexes)
                    res += network.HiddenCount;
            }
            return res;
        }

        public static IList<Parameter> GetParameters(this RBFNetwork network, NetworkParameters parameters)
        {
            var result = new List<Parameter>();

            if (parameters.UseWeights)
                result.AddRange(network.Weights);
            if (parameters.UseCenters)
            {
                for (int i = 0; i < network.InputCount; i++)
                    result.AddRange(network.Functions.Select(f => f.Center[i]));
            }
            var indexes = parameters.UseAllParmeters ? Enumerable.Range(0, network.Functions[0].Parameters.Length) : parameters.Indexes;
            if (indexes.Any())
            {
                foreach (var index in indexes)
                    result.AddRange(network.Functions.Select(f => f.Parameters[index]));
            }
            return result;
        }

        public static Point GetPoint(this RBFNetwork network, NetworkParameters parameters)
        {
            return new Point(GetParameters(network, parameters).Select(p => p.Value).ToArray());
        }

        public static Parameter GetParameterByCoordinate(this RBFNetwork network, int coordinate, NetworkParameters parameters)
        {
            var hiddenCount = network.HiddenCount;
            var seek = 0;

            if (parameters.UseWeights)
            {
                if (coordinate < seek + hiddenCount)
                    return network.Weights[coordinate];
                seek += hiddenCount;
            }
            if (parameters.UseCenters)
            {
                for (int i = 0; i < network.InputCount; i++)
                {
                    if (coordinate < seek + hiddenCount)
                        return network.Functions[coordinate - seek].Center[i];
                    seek += hiddenCount;
                }
            }
            var indexes = parameters.UseAllParmeters ? Enumerable.Range(0, network.Functions[0].Parameters.Length) : parameters.Indexes;
            if (indexes.Any())
            {
                foreach (var index in indexes)
                {
                    if (coordinate < seek + hiddenCount)
                        return network.Functions[coordinate - seek].Parameters[index];
                    seek += hiddenCount;
                }
            }
            
            return null;
        }

        public static void ClearHistory(this RBFNetwork network, NetworkParameters parameters)
        {
            var undoW = parameters.UseWeights;

            var indexes = parameters.UseAllParmeters ? Enumerable.Range(0, network.Functions[0].Parameters.Length) : parameters.Indexes;
            var undoP = indexes.Any();

            var undoC = parameters.UseCenters;

            for (var i = 0; i < network.HiddenCount; i++)
            {
                if (undoW)
                    network.Weights[i].Clear();
                if (undoP)
                {
                    foreach (var index in indexes)
                        network.Functions[i].Parameters[index].Clear();
                }
                if (undoC)
                {
                    for (int i1 = 0; i1 < network.InputCount; i1++)
                        network.Functions[i].Center[i1].Clear();
                }
            }
        }

        public static void MoveToPoint(this RBFNetwork network, Point point, NetworkParameters parameters)
        {
            GetParameters(network, parameters).Select((p, i) => new { Index = i, Parameter = p }).ToList()
                .ForEach(v => v.Parameter.Value = point[v.Index]);
        }

        public static void MoveBack(this RBFNetwork network, NetworkParameters parameters)
        {
            var undoW = parameters.UseWeights;

            var indexes = parameters.UseAllParmeters ? Enumerable.Range(0, network.Functions[0].Parameters.Length) : parameters.Indexes;
            var undoP = indexes.Any();

            var undoC = parameters.UseCenters;

            for (var i = 0; i < network.HiddenCount; i++)
            {
                if (undoW)
                    network.Weights[i].Undo();
                if (undoP)
                {
                    foreach (var index in indexes)
                        network.Functions[i].Parameters[index].Undo();
                }
                if (undoC)
                {
                    for (int i1 = 0; i1 < network.InputCount; i1++)
                        network.Functions[i].Center[i1].Undo();
                }
            }
        }
    }
}




