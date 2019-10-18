// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to png.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Core.Drawing
{
    using System.Drawing;
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
        /// Gets or sets the background color of the exported png.
        /// </summary>
        public OxyColor Background { get; set; }

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
        /// <param name="background">The background color.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, OxyColor background, double resolution = 96)
        {
            var exporter = new PngExporter { Width = width, Height = height, Background = background, Resolution = resolution };
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
            using (var bm = this.ExportToBitmap(model))
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Bitmap" />.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <returns>A <see cref="Bitmap"/>.</returns>
        public Bitmap ExportToBitmap(IPlotModel model)
        {
            var bm = new Bitmap(this.Width, this.Height);
            using (var g = Graphics.FromImage(bm))
            {
                if (!this.Background.IsInvisible())
                {
                    g.FillRectangle(this.Background.ToBrush(), 0, 0, this.Width, this.Height);
                }

                using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                {
                    model.Update(true);
                    model.Render(rc, this.Width, this.Height);
                }

                // this throws an exception
                // bm.SetResolution(resolution, resolution);
                // https://github.com/dotnet/corefx/blob/master/src/System.Drawing.Common/src/System/Drawing/Bitmap.cs#L301

                return bm;
            }
        }
    }
}
