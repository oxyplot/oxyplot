// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasPlotView.cs" company="OxyPlot">
//   Copyright (c) 2014-2026 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Windows;
    using System.Windows.Controls;

    using OxyPlot;
    using OxyPlot.Wpf;

    /// <summary>
    /// Represents a PlotView which uses the CanvasRenderContext for rendering.
    /// </summary>
    public class CanvasPlotView : PlotView
    {
        public CanvasPlotView()
        {
            this.DisconnectCanvasWhileUpdating = true;
        }

        /// <inheritdoc/>
        protected override bool RenderSurfaceHandlesMouseEvents => false;

        protected override FrameworkElement CreatePlotPresenter()
        {
            return new Canvas();
        }

        protected override IRenderContext CreateRenderContext()
        {
            return new CanvasRenderContext((Canvas)this.RenderSurface);
        }

        protected override void RenderOverride()
        {
            int idx = -1;
            if (this.DisconnectCanvasWhileUpdating)
            {
                // TODO: profile... not sure if this makes any difference
                idx = this.grid.Children.IndexOf(this.plotPresenter);
                if (idx != -1)
                {
                    this.grid.Children.RemoveAt(idx);
                }
            }

            base.RenderOverride();

            if (idx != -1)
            {
                // reinsert the canvas again
                this.grid.Children.Insert(idx, this.plotPresenter);
            }
        }
    }
}
