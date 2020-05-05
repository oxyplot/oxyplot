// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JpegExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to jpeg.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp
{
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to jpeg.
    /// </summary>
    public class JpegExporter : IExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JpegExporter"/> class.
        /// </summary>
        /// <param name="width">The width in pixels of the exported jpeg.</param>
        /// <param name="height">The height in pixels of the exported jpeg.</param>
        /// <param name="resolution">The resolution in dots per inch (DPI) of the exported jpeg.</param>
        /// <param name="quality">The quality of the exported jpeg, a value between 0 and 100.</param>
        public JpegExporter(int width, int height, double resolution = 96, int quality = 75)
        {
            this.Width = width;
            this.Height = height;
            this.Resolution = resolution;
            this.Quality = quality;
        }

        /// <summary>
        /// Gets or sets the width in pixels of the exported jpeg.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels of the exported jpeg.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the resolution in dots per inch (DPI) of the exported jpeg.
        /// </summary>
        public double Resolution { get; set; }

        /// <summary>
        /// Gets or sets the quality of the exported jpeg, a value between 0 and 100.
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, double resolution = 96)
        {
            var exporter = new JpegExporter(width, height, resolution);
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
        public void Export(IPlotModel model, Stream stream)
        {
            var background = model.Background.IsInvisible() ? OxyColors.White : model.Background;
            using (var rc = new ImageRenderContext(this.Width, this.Height, background, this.Resolution))
            {
                var dpiScale = this.Resolution / 96;
                model.Update(true);
                model.Render(rc, new OxyRect(0, 0, this.Width / dpiScale, this.Height / dpiScale));
                rc.SaveAsJpeg(stream, this.Quality);
            }
        }
    }
}
