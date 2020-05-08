// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegExporter.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using global::SkiaSharp;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to jpeg using the SkiaSharp renderer.
    /// </summary>
    public class JpegExporter : IExporter
    {
        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        public float Dpi { get; set; } = 96;

        /// <summary>
        /// Gets or sets the export height (in pixels).
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the export width (in pixels).
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the export quality (0-100).
        /// </summary>
        public int Quality { get; set; } = 90;

        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="path">The path.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="quality">The export quality (0-100).</param>
        /// <param name="dpi">The DPI (dots per inch).</param>
        public static void Export(IPlotModel model, string path, int width, int height, int quality, float dpi = 96)
        {
            using var stream = File.OpenWrite(path);
            Export(model, stream, width, height, quality, dpi);
        }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="quality">The export quality (0-100).</param>
        /// <param name="dpi">The DPI (dots per inch).</param>
        public static void Export(IPlotModel model, Stream stream, int width, int height, int quality, float dpi = 96)
        {
            var exporter = new JpegExporter { Width = width, Height = height, Quality = quality, Dpi = dpi };
            exporter.Export(model, stream);
        }

        /// <inheritdoc/>
        public void Export(IPlotModel model, Stream stream)
        {
            using var bitmap = new SKBitmap(this.Width, this.Height);

            using (var canvas = new SKCanvas(bitmap))
            using (var context = new SkiaRenderContext { RenderTarget = RenderTarget.PixelGraphic, SkCanvas = canvas })
            {
                canvas.Clear(SKColors.White);
                var dpiScale = this.Dpi / 96;
                context.DpiScale = dpiScale;
                model.Update(true);
                var backgroundColor = model.Background;

                // jpg doesn't support transparency
                if (!backgroundColor.IsVisible())
                {
                    backgroundColor = OxyColors.White;
                }

                canvas.Clear(backgroundColor.ToSKColor());
                model.Render(context, new OxyRect(0, 0, this.Width / dpiScale, this.Height / dpiScale));
            }

            using var skStream = new SKManagedWStream(stream);
            SKPixmap.Encode(skStream, bitmap, SKEncodedImageFormat.Jpeg, this.Quality);
        }
    }
}
