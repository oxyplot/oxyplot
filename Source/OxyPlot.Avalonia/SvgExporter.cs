// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to scalable vector graphics using text measuring in Avalonia.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia.Controls;

namespace OxyPlot.Avalonia
{

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics using text measuring in Avalonia.
    /// </summary>
    public class SvgExporter : OxyPlot.SvgExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter" /> class.
        /// </summary>
        public SvgExporter()
        {
            TextMeasurer = new CanvasRenderContext(new Canvas());
        }
    }
}