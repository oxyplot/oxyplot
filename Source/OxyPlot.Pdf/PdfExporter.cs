// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporter.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
//   Exporting PlotModels to PDF.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Pdf
{
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to portable document format.
    /// </summary>
    public static class PdfExporter
    {
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
            using (FileStream s = File.Create(path))
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
        /// <param name="s">
        /// The stream.
        /// </param>
        /// <param name="width">
        /// The width (points).
        /// </param>
        /// <param name="height">
        /// The height (points).
        /// </param>
        public static void Export(PlotModel model, Stream s, double width, double height)
        {
            using (var rc = new PdfRenderContext(width, height, model.Background))
            {
                model.Update();
                model.Render(rc, width, height);
                rc.Save(s);
            }
        }
    }
}