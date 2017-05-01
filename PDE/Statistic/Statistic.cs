using System;
using System.Collections.Generic;
using RBFNetwork.Train;

namespace PDE.Statistic
{
    public class Statistic
    {
        private IDictionary<string, IList<StatisticCollector>> _statisticCollectors = new Dictionary<string, IList<StatisticCollector>>();
        
        public void AddStatisticCollector(string group, StatisticCollector collector)
        {
            if(!_statisticCollectors.ContainsKey(group))
                _statisticCollectors.Add(group, new List<StatisticCollector>());
            _statisticCollectors[group].Add(collector);
        }

        public void Collect(string group, Trainer.TrainStateArg state)
        {
            if (_statisticCollectors.ContainsKey(group))
                foreach (var statisticCollector in _statisticCollectors[group])
                    statisticCollector.Collect(state);
        }

        public void ExecuteExtention<T>(string group) where T : IExtention
        {
            if (!_statisticCollectors.ContainsKey(group))
                return;
            foreach (var statisticCollector in _statisticCollectors[group])
            {
                statisticCollector.ExecuteExtention<T>();
            }
        }
    }
}
