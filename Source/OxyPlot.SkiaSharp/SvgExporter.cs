// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using System;

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics using SkiaSharp-based text measuring.
    /// </summary>
    public sealed class SvgExporter : OxyPlot.SvgExporter, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter" /> class, using a <see cref="SkiaRenderContext"/> as text measurer.
        /// </summary>
        public SvgExporter()
        {
            this.TextMeasurer = new SkiaRenderContext();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            var skiaMeasurer = this.TextMeasurer as SkiaRenderContext;
            skiaMeasurer?.Dispose();
            this.TextMeasurer = null;
        }
    }
}
