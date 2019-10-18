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
        /// Exports the specified <see cref="PlotModel" /> to the specified file.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="filename">
        /// The file name.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="background">
        /// The background color.
        /// </param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static void Export(IPlotModel model, string filename, int width, int height, OxyColor background, double resolution = 96)
        {
            using (var bm = ExportToBitmap(model, width, height, background, resolution))
            {
                bm.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static void Export(IPlotModel model, Stream stream, int width, int height, OxyColor background, double resolution = 96)
        {
            using (var bm = ExportToBitmap(model, width, height, background, resolution))
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        public static Stream ExportToStream(IPlotModel model, int width, int height, OxyColor background, double resolution = 96)
        {
            var stream = new MemoryStream();
            using (var bm = ExportToBitmap(model, width, height, background, resolution))
            {
                bm.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                return stream;
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Bitmap" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        /// <param name="resolution">The resolution in dpi (defaults to 96dpi).</param>
        /// <returns>A bitmap.</returns>
        public static Bitmap ExportToBitmap(IPlotModel model, int width, int height, OxyColor background, double resolution = 96)
        {
            var bm = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bm))
            {
                if (background.IsVisible())
                {
                    using (var brush = background.ToBrush())
                    {
                        g.FillRectangle(brush, 0, 0, width, height);
                    }
                }

                using (var rc = new GraphicsRenderContext(g) { RendersToScreen = false })
                {
                    model.Update(true);
                    model.Render(rc, width, height);
                }

                // this throws an exception
                // bm.SetResolution(resolution, resolution);
                // https://github.com/dotnet/corefx/blob/master/src/System.Drawing.Common/src/System/Drawing/Bitmap.cs#L301

                return bm;
            }
        }

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

        /// <inheritdoc />
        public void Export(IPlotModel model, Stream stream) => Export(model, stream, this.Width, this.Height, this.Background, this.Resolution);

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a file.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="filename">The file path.</param>
        public void ExportToFile(IPlotModel model, string filename) => Export(model, filename, this.Width, this.Height, this.Background, this.Resolution);
    }
}
