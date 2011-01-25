using System.Collections.ObjectModel;

namespace OxyPlot
{
    public enum AnnotationLayer
    {
        BelowSeries,
        OverSeries
    }

    public interface IAnnotation
    {
        AnnotationLayer Layer { get; set; }
        void Render(IRenderContext rc, PlotModel model);
        void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis);
    }
}