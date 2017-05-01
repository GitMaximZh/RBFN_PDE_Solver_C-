using System;
using PDE.Statistic;

namespace Solver.Statistic
{
    internal class ShowSliceRelativeError : Extention<SliceRelativeErrorCollector>, IShowExtention
    {
        public ShowSliceRelativeError(SliceRelativeErrorCollector extendable)
            : base(extendable)
        {
        }

        public override void Execute()
        {
            Displayer.Instance.AddToDisplay("Slice error: " + Extendable.RelativeError);
        }
    }
}
