namespace OxyPlot
{
    public interface ISeries
    {
        void Render(IRenderContext rc, PlotModel model);
        string Title { get; }
    }
}