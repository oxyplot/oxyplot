// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to scalable vector graphics using <see cref="Graphics" /> for text measuring.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics using ImageSharp for text measuring.
    /// </summary>
    public class SvgExporter : OxyPlot.SvgExporter, IDisposable
    {
        /// <summary>
        /// The render context.
        /// </summary>
        private ImageRenderContext irc;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="resolution">The resolution in dots per inch.</param>
        public SvgExporter(double width, double height, double resolution = 96)
        {
            this.Width = width;
            this.Height = height;
            this.irc = new ImageRenderContext(1, 1, OxyColors.Undefined, resolution);
            this.TextMeasurer = this.irc;
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, double resolution = 96)
        {
            var exporter = new SvgExporter(width, height, resolution);
            using (var stream = File.Create(fileName))
            {
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the <see cref="SvgExporter"/>.
        /// </summary>
        /// <param name="disposing">Whether we are disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.irc.Dispose();
            }
        }
    }
}
