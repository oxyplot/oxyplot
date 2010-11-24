using System.Windows;
using System.Windows.Media;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Control that renders the PlotModel using a DrawingContext.
    /// This should give the highest performance with WPF.
    /// The problem is currently to mix aliased (axes) and anti-aliased (curves) elements...
    /// Currently we are rendering to two different PlotFrames, with and without the Alias property set.
    /// todo: any good ideas to do this better?
    /// </summary>
    internal class PlotFrame : FrameworkElement
    {
        private readonly bool Aliased;

        public PlotFrame(bool aliased = false)
        {
            Aliased = aliased;
            SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            if (Aliased)
                SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        public PlotModel Model { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Model == null)
                return;

            var drc = new DrawingRenderContext(dc, ActualWidth, ActualHeight);

            // todo: this is not working properly...
            if (Aliased)
            {
                Model.RenderInit(drc);
                Model.RenderAxes(drc);
                Model.RenderBox(drc);
            }
            else
            {
                Model.RenderSeries(drc);
            }
        }
    }
}