// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to scalable vector graphics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics.
    /// </summary>
    public class SvgExporter : IExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter" /> class.
        /// </summary>
        public SvgExporter()
        {
            this.Width = 600;
            this.Height = 400;
            this.IsDocument = true;
        }

        /// <summary>
        /// Gets or sets the width (in user units) of the output area.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height (in user units) of the output area.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the xml headers should be included.
        /// </summary>
        public bool IsDocument { get; set; }

        /// <summary>
        /// Gets or sets the text measurer.
        /// </summary>
        public IRenderContext TextMeasurer { get; set; }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <param name="textMeasurer">The text measurer.</param>
        public static void Export(IPlotModel model, Stream stream, double width, double height, bool isDocument, IRenderContext textMeasurer = null)
        {
            if (textMeasurer == null)
            {
                textMeasurer = new PdfRenderContext(width, height, model.Background);
            }

            using (var rc = new SvgRenderContext(stream, width, height, true, textMeasurer, model.Background))
            {
                model.Update(true);
                model.Render(rc, width, height);
                rc.Complete();
                rc.Flush();
            }
        }

        /// <summary>
        /// Exports to string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <param name="textMeasurer">The text measurer.</param>
        /// <returns>The plot as an <c>SVG</c> string.</returns>
        public static string ExportToString(IPlotModel model, double width, double height, bool isDocument, IRenderContext textMeasurer = null)
        {
            string svg;
            using (var ms = new MemoryStream())
            {
                Export(model, ms, width, height, isDocument, textMeasurer);
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                svg = sr.ReadToEnd();
            }

            return svg;
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="stream">The target stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            Export(model, stream, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>the SVG content as a string.</returns>
        public string ExportToString(IPlotModel model)
        {
            return ExportToString(model, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
        }
    }
}