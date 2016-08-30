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
        /// Gets or sets the resolution of the output image.
        /// </summary>
        /// <value>The resolution in dots per inch (dpi).</value>
        public int Resolution { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Exports the specified plot model to a file.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width of the output bitmap.</param>
        /// <param name="height">The height of the output bitmap.</param>
        /// <param name="background">The background color. The default value is <c>null</c>.</param>
        /// <param name="resolution">The resolution (resolution). The default value is 96.</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, OxyColor background, int resolution = 96)
        {
            using (var s = File.Create(fileName))
            {
                Export(model, s, width, height, background, resolution);
            }
        }

        /// <summary>
        /// Exports the specified plot model to a stream.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width of the output bitmap.</param>
        /// <param name="height">The height of the output bitmap.</param>
        /// <param name="background">The background color. The default value is <c>null</c>.</param>
        /// <param name="resolution">The resolution (resolution). The default value is 96.</param>
        public static void Export(IPlotModel model, Stream stream, int width, int height, OxyColor background, int resolution = 96)
        {
            var exporter = new PngExporter { Width = width, Height = height, Background = background, Resolution = resolution };
            exporter.Export(model, stream);
        }

        /// <summary>
        /// Exports the specified plot model to a bitmap.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <param name="resolution">The resolution (dpi).</param>
        /// <returns>A bitmap.</returns>
        public static BitmapSource ExportToBitmap(
            IPlotModel model,
            int width,
            int height,
            OxyColor background,
            int resolution = 96)
        {
            var exporter = new PngExporter { Width = width, Height = height, Background = background, Resolution = resolution };
            return exporter.ExportToBitmap(model);
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
            var canvas = new Canvas { Width = this.Width * scale, Height = this.Height * scale, Background = this.Background.ToBrush() };
            canvas.Measure(new Size(canvas.Width, canvas.Height));
            canvas.Arrange(new Rect(0, 0, canvas.Width, canvas.Height));

            var rc = new CanvasRenderContext(canvas) { RendersToScreen = false };

            rc.TextFormattingMode = TextFormattingMode.Ideal;

            model.Update(true);
            model.Render(rc, canvas.Width, canvas.Height);

            canvas.UpdateLayout();

            var bmp = new RenderTargetBitmap(this.Width, this.Height, this.Resolution, this.Resolution, PixelFormats.Pbgra32);
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