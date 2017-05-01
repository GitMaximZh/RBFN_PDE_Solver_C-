namespace RBFNetwork.Train
{
    public interface IControlPointsTransformer
    {
        bool ShouldBeTransformed(int step);
        IControlPoint[] Transform(IControlPoint[] points);
    }
}
