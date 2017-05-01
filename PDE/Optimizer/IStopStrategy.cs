namespace PDE.Optimizer
{
    public interface IStopStrategy
    {
        bool ShouldBeStoped(int step, double error, double previousError);
    }
}
