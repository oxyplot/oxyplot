// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotFrame.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that renders a PlotModel using a DrawingContext.
//   This should give the highest performance with WPF.
//   The problem is currently to mix aliased (axes) and anti-aliased (curves) elements...
//   Currently we are rendering to two different PlotFrames, with and without the Alias property set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#if RENDER_BY_DRAWINGCONTEXT
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control that renders a PlotModel using a DrawingContext.
    /// This should give the highest performance with WPF.
    /// The problem is currently to mix aliased (axes) and anti-aliased (curves) elements...
    /// Currently we are rendering to two different PlotFrames, with and without the Alias property set.
    /// </summary>
    internal class PlotFrame : FrameworkElement
    {
        private readonly bool aliased;

        public bool Aliased
        {
            get { return aliased; }
        }

        public PlotFrame(bool aliased = false)
        {
            this.aliased = aliased;
            SetValue(RenderOptions.ClearTypeHintProperty, ClearTypeHint.Enabled);

            if (this.aliased)
                SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        public PlotModel Model { get; set; }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (Model == null)
                return;

            var drc = new DrawingRenderContext(dc, ActualWidth, ActualHeight);

            if (aliased)
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
#endif