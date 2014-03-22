// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
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
//   Provides functionality to export plots to png.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to png.
    /// </summary>
    public class PngExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PngExporter"/> class.
        /// </summary>
        public PngExporter()
        {
            this.Width = 700;
            this.Height = 400;
            this.Resolution = 96;
            this.Background = OxyColors.White;
        }

        /// <summary>
        /// Gets or sets the width of the output image.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the output image.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the resolution (dpi) of the output image.
        /// </summary>
        public int Resolution { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Exports the specified model.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="background">
        /// The background.
        /// </param>
        public static void Export(PlotModel model, string fileName, int width, int height, Brush background = null)
        {
            var exporter = new PngExporter { Width = width, Height = height, Background = background.ToOxyColor() };
            using (var stream = File.Create(fileName))
            {
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        public void Export(PlotModel model, Stream stream)
        {
            using (var bm = new Bitmap(this.Width, this.Height))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    if (this.Background.IsVisible())
                    {
                        using (var brush = this.Background.ToBrush())
                        {
                            g.FillRectangle(brush, 0, 0, this.Width, this.Height);
                        }
                    }

                    using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                    {
                        model.Update();
                        model.Render(rc, this.Width, this.Height);
                    }

                    bm.Save(stream, ImageFormat.Png);
                }
            }
        }
    }
}