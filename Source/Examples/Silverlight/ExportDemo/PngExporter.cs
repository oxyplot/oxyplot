// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
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
//   Export a PlotModel to .png using ImageTools
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExportDemo
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using ImageTools;

    using OxyPlot;
    using OxyPlot.Silverlight;

    /// <summary>
    /// Exports a PlotModel to .png using ImageTools
    /// </summary>
    public static class PngExporter
    {
        /// <summary>
        /// Exports the specified plot model to a stream.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="width">The width of the export image.</param>
        /// <param name="height">The height of the exported image.</param>
        /// <param name="background">The background.</param>
        public static void Export(PlotModel model, Stream stream, double width, double height, OxyColor background = null)
        {
            var canvas = new Canvas { Width = width, Height = height };
            if (background != null)
            {
                canvas.Background = background.ToBrush();
            }

            canvas.Measure(new Size(width, height));
            canvas.Arrange(new Rect(0, 0, width, height));

            var rc = new SilverlightRenderContext(canvas);
            model.Update();
            model.Render(rc, width, height);

            canvas.UpdateLayout();
            var image = canvas.ToImage();
            image.WriteToStream(stream);
        }
    }
}