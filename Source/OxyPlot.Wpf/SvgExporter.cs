// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to scalable vector graphics using text measuring in WPF.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows.Controls;

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics using text measuring in WPF.
    /// </summary>
    public class SvgExporter : OxyPlot.SvgExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter" /> class.
        /// </summary>
        public SvgExporter()
        {
            this.TextMeasurer = new CanvasRenderContext(new Canvas());
        }
    }
}