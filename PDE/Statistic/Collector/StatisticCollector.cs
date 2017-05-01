namespace PDE.Statistic
{
    public abstract class StatisticCollector : IStatisticCollector, IExtendable
    {
        private readonly IExtendable _extendable;

        protected StatisticCollector()
        {
            _extendable = new Extendable();
        }

        public abstract void Collect(RBFNetwork.Train.Trainer.TrainStateArg trainState);

        public void AddExtention(IExtention extention)
        {
            _extendable.AddExtention(extention);
        }

        public void ExecuteExtention<T>() where T : IExtention
        {
            _extendable.ExecuteExtention<T>();
        }
    }
}
