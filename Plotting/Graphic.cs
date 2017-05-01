namespace Plotting
{
    public class Graphic
    {
        public Function[] Functions { get; private set; }
        public Graphic(params Function[] functions)
        {
            Functions = functions;
        }        
    }
}
