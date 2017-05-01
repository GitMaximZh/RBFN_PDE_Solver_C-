namespace PDE.Optimizer
{
    public class StopStrategy : IStopStrategy
    {
        public double Epsilon { get; set; }

        public StopStrategy()
        {
            Epsilon = 0.00001;
        }

        public bool ShouldBeStoped(int step, double error, double previousError)
        {
            return previousError - error < Epsilon;
        }
    }
}
