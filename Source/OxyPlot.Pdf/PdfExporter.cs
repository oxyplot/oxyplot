// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporter.cs" company="OxyPlot">
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
//   Provides functionality to export plots to pdf.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to pdf.
    /// </summary>
    public class PdfExporter
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
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="width">
        /// The width (points).
        /// </param>
        /// <param name="height">
        /// The height (points).
        /// </param>
        public static void Export(PlotModel model, string path, double width, double height)
        {
            using (var s = File.Create(path))
            {
                Export(model, s, width, height);
            }
        }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="stream">
        /// The output stream.
        /// </param>
        /// <param name="width">
        /// The width (points).
        /// </param>
        /// <param name="height">
        /// The height (points).
        /// </param>
        public static void Export(PlotModel model, Stream stream, double width, double height)
        {
            var exporter = new PdfExporter { Width = width, Height = height, Background = model.Background };
            exporter.Export(model, stream);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        public void Export(PlotModel model, Stream stream)
        {
            using (var rc = new PdfRenderContext(this.Width, this.Height, this.Background))
            {
                model.Update();
                model.Render(rc, this.Width, this.Height);
                rc.Save(stream);
            }
        }
    }
}