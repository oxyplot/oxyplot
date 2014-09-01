// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods to the <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Provides extension methods to the <see cref="PlotModel" />.
    /// </summary>
    public static class PlotModelExtensions
    {
        /// <summary>
        /// Creates an SVG string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <returns>A <see cref="string" />.</returns>
        public static string ToSvg(this PlotModel model, double width, double height, bool isDocument)
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                {
                    return OxyPlot.SvgExporter.ExportToString(model, width, height, isDocument, rc);
                }
            }
        }
    }
}