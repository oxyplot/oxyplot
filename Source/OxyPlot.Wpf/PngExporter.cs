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
//   Export a PlotModel to .png using WPF
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Export a PlotModel to .png using WPF
    /// </summary>
    public static class PngExporter
    {
        /// <summary>
        /// Exports the specified plot model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <param name="resolution">The resolution.</param>
        public static void Export(PlotModel model, string fileName, int width, int height, OxyColor background = null, int resolution = 96)
        {
            using (var s = File.Create(fileName))
            {
                Export(model, s, width, height, background, resolution);
            }
        }

        /// <summary>
        /// Exports the specified plot model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <param name="resolution">The resolution.</param>
        public static void Export(PlotModel model, Stream stream, int width, int height, OxyColor background = null, int resolution = 96)
        {
            var bmp = ExportToBitmap(model, width, height, background);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            encoder.Save(stream);
        }

        /// <summary>
        /// Exports the specified plot model to a bitmap.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <param name="dpi">The resolution.</param>
        /// <returns>A bitmap.</returns>
        public static BitmapSource ExportToBitmap(PlotModel model, int width, int height, OxyColor background = null, int dpi = 96)
        {
            var canvas = new Canvas { Width = width, Height = height, Background = background.ToBrush() };
            canvas.Measure(new Size(width, height));
            canvas.Arrange(new Rect(0, 0, width, height));

            var rc = new ShapesRenderContext(canvas) { RendersToScreen = false };
            model.Update();
            model.Render(rc, width, height);

            canvas.UpdateLayout();

            var bmp = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Pbgra32);
            bmp.Render(canvas);
            return bmp;

            // alternative implementation:
            // http://msdn.microsoft.com/en-us/library/system.windows.media.imaging.rendertargetbitmap.aspx
            // var dv = new DrawingVisual();
            // using (var ctx = dv.RenderOpen())
            // {
            //    var vb = new VisualBrush(canvas);
            //    ctx.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            // }
            // bmp.Render(dv);
        }
    }
}