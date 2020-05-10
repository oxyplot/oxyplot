// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to pdf.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to pdf.
    /// </summary>
    [Obsolete("OxyPlot.PdfExporter may be removed in a future version. Consider using OxyPlot.SkiaSharp.PdfExporter instead.")]
    public class PdfExporter : IExporter
    {
        /// <summary>
        /// Gets or sets the width (in points, 1/72 inch) of the output document.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height (in points, 1/72 inch) of the output document.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(IPlotModel model, Stream stream, double width, double height)
        {
            var exporter = new PdfExporter { Width = width, Height = height };
            exporter.Export(model, stream);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            var rc = new PdfRenderContext(this.Width, this.Height, model.Background);
            model.Update(true);
            model.Render(rc, new OxyRect(0, 0, this.Width, this.Height));
            rc.Save(stream);
        }
    }
}
