// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfExporter.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using global::SkiaSharp;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to pdf using the SkiaSharp renderer.
    /// </summary>
    public class PdfExporter : IExporter
    {
        /// <summary>
        /// Gets or sets the export height (in points, where 1 point equals 1/72 inch).
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the export width (in points, where 1 point equals 1/72 inch).
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether text shaping should be used when rendering text.
        /// </summary>
        public bool UseTextShaping { get; set; } = true;

        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="path">The path.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(IPlotModel model, string path, float width, float height)
        {
            using var stream = File.OpenWrite(path);
            Export(model, stream, width, height);
        }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(IPlotModel model, Stream stream, float width, float height)
        {
            var exporter = new PdfExporter { Width = width, Height = height };
            exporter.Export(model, stream);
        }

        /// <inheritdoc/>
        public void Export(IPlotModel model, Stream stream)
        {
            using var document = SKDocument.CreatePdf(stream);
            using var pdfCanvas = document.BeginPage(this.Width, this.Height);
            using var context = new SkiaRenderContext { RenderTarget = RenderTarget.VectorGraphic, SkCanvas = pdfCanvas, UseTextShaping = this.UseTextShaping };
            const float dpiScale = 72f / 96;
            context.DpiScale = dpiScale;
            model.Update(true);
            pdfCanvas.Clear(model.Background.ToSKColor());
            model.Render(context, new OxyRect(0, 0, this.Width / dpiScale, this.Height / dpiScale));
        }
    }
}
