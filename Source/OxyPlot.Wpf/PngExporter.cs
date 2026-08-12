// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to png.
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
    /// Provides functionality to export plots to png.
    /// </summary>
    public class PngExporter : IExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PngExporter" /> class.
        /// </summary>
        public PngExporter()
        {
            this.Width = 700;
            this.Height = 400;
            this.Resolution = 96;
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
        /// Gets or sets the resolution of the output image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public double Resolution { get; set; }

        /// <summary>
        /// Exports the specified plot model to a file.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width of the output bitmap.</param>
        /// <param name="height">The height of the output bitmap.</param>
        /// <param name="resolution">The resolution (resolution). The default value is 96.</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, double resolution = 96)
        {
            var exporter = new PngExporter { Width = width, Height = height, Resolution = resolution };
            using (var stream = File.Create(fileName))
            {
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            var bmp = this.ExportToBitmap(model);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);
        }

        /// <summary>
        /// Exports the specified plot model to a bitmap.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <returns>A bitmap.</returns>
        public BitmapSource ExportToBitmap(IPlotModel model)
        {
            var scale = 96d / this.Resolution;
            var renderSurface = new RenderSurface { Width = this.Width * scale, Height = this.Height * scale, Background = model.Background.ToBrush() };

            renderSurface.Measure(new Size(renderSurface.Width, renderSurface.Height));
            renderSurface.Arrange(new Rect(0, 0, renderSurface.Width, renderSurface.Height));

            var rc = new DrawingRenderContext(renderSurface) { RendersToScreen = false };

            rc.TextFormattingMode = TextFormattingMode.Ideal;
            rc.DpiScale = this.Resolution / 96;

            model.Update(true);
            try
            {
                renderSurface.BeginRender();
                model.Render(rc, new OxyRect(0, 0, renderSurface.Width, renderSurface.Height));
            }
            finally
            {
                renderSurface.EndRender();
            }

            renderSurface.UpdateLayout();

            var bmp = new RenderTargetBitmap(this.Width, this.Height, this.Resolution, this.Resolution, PixelFormats.Pbgra32);
            bmp.Render(renderSurface);
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
