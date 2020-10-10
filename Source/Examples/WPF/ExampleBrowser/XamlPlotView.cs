// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlPlotView.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using OxyPlot;
    using OxyPlot.Wpf;

    /// <summary>
    /// Represents a PlotView which uses the XamlRenderContext for rendering.
    /// </summary>
    public class XamlPlotView : PlotView
    {
        protected override IRenderContext CreateRenderContext()
        {
            return new XamlRenderContext(this.Canvas);
        }
    }
}
