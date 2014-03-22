// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    public class SvgExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SvgExporter"/> class.
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
        public static void Export(PlotModel model, Stream stream, double width, double height, bool isDocument, IRenderContext textMeasurer = null)
        {
            if (textMeasurer == null)
            {
                textMeasurer = new PdfRenderContext(width, height, model.Background);
            }

            using (var rc = new SvgRenderContext(stream, width, height, true, textMeasurer, model.Background))
            {
                model.Update();
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
        /// <returns>
        /// The plot as a svg string.
        /// </returns>
        public static string ExportToString(PlotModel model, double width, double height, bool isDocument, IRenderContext textMeasurer = null)
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
        /// Exports the specified <see cref="PlotModel"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="stream">The target stream.</param>
        public void Export(PlotModel model, Stream stream)
        {
            Export(model, stream, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel"/> to a string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>the SVG content as a string.</returns>
        public string ExportToString(PlotModel model)
        {
            return ExportToString(model, this.Width, this.Height, this.IsDocument, this.TextMeasurer);
        }
    }
}