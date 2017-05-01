using System;

namespace PDE.Optimizer
{
    public class CommonSubOptimizer : SubOptimizer
    {
        private readonly Func<int, bool> _shouldOptimize;

        public bool ResetOnNetworkChaged { get; set; }

        public CommonSubOptimizer(Func<int, bool> shouldOptimize,
            IOpimizationMethodFactory opimizationMethodFactory)
            : base(opimizationMethodFactory)
        {
            _shouldOptimize = shouldOptimize;
        }

        protected override bool ShouldOptimize(int step)
        {
            return _shouldOptimize(step);
        }

        public override void OnNetworkChanged()
        {
            if (ResetOnNetworkChaged)
                _optimizeMethod = null;
        }
    }
}
