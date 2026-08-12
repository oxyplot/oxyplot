// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlPlotView.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System.Windows;
    using System.Windows.Controls;

    using OxyPlot;
    using OxyPlot.Wpf;

    /// <summary>
    /// Represents a PlotView which uses the XamlRenderContext for rendering.
    /// </summary>
    public class XamlPlotView : CanvasPlotView
    {
        protected override IRenderContext CreateRenderContext()
        {
            return new XamlRenderContext((Canvas)this.RenderSurface);
        }
    }
}
