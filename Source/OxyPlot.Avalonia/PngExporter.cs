// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to png.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia
{
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Media.Imaging;
    using global::Avalonia.Rendering;
    using System.IO;

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
            Width = 700;
            Height = 400;
            Resolution = 96;
            Background = OxyColors.White;
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
        public static IBitmap ExportToBitmap(
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
            var bmp = ExportToBitmap(model);
            bmp.Save(stream);
        }

        /// <summary>
        /// Exports the specified plot model to a bitmap.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <returns>A bitmap.</returns>
        public IBitmap ExportToBitmap(IPlotModel model)
        {
            var scale = 96d / Resolution;
            var canvas = new Canvas { Width = Width * scale, Height = Height * scale, Background = Background.ToBrush() };
            canvas.Measure(new Size(canvas.Width, canvas.Height));
            canvas.Arrange(new Rect(0, 0, canvas.Width, canvas.Height));

            var rc = new CanvasRenderContext(canvas) { RendersToScreen = false };
            
            model.Update(true);
            model.Render(rc, canvas.Width, canvas.Height);

            canvas.Measure(new Size(canvas.Width, canvas.Height));
            canvas.Arrange(new Rect(0, 0, canvas.Width, canvas.Height));

            var bmp = new RenderTargetBitmap(Width, Height);
            bmp.Render(canvas);
            return bmp;
        }
    }
}