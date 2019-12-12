// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to png.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp
{
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to png.
    /// </summary>
    public class PngExporter : IExporter
    {
        /// <summary>
        /// Gets or sets the width in pixels of the exported png.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height in pixels of the exported png.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the resolution in dpi of the exported png.
        /// </summary>
        public double Resolution { get; set; }

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
            var exporter = new PngExporter { Width = width, Height = height, Resolution = resolution };
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
            using (var rc = new PngRenderingContext(this.Width, this.Height, model.Background, this.Resolution))
            {
                model.Update(true);
                model.Render(rc, new OxyRect(0, 0, this.Width, this.Height));
                rc.Save(stream);
            }
        }
    }
}
