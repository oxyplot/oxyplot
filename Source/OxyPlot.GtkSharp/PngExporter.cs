// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a png exporter based on GTK#.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.GtkSharp
{
    using System;
    using System.IO;

    using Cairo;

    /// <summary>
    /// Provides a png exporter based on GTK#.
    /// </summary>
    public class PngExporter
    {
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
        public int Resolution { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a png file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the output file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        public static void Export(IPlotModel model, string fileName, int width, int height, Pattern background = null)
        {
            using (var bm = new ImageSurface(Format.ARGB32, width, height))
            {
                using (var g = new Context(bm))
                {
                    if (background != null)
                    {
                        g.Save();
                        g.SetSource(background);
                        g.Rectangle(0, 0, width, height);
                        g.Fill();
                        g.Restore();
                    }

                    var rc = new GraphicsRenderContext { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    model.Update(true);
                    model.Render(rc, width, height);
                    bm.WriteToPng(fileName);
                }
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            using (var bm = new ImageSurface(Format.ARGB32, this.Width, this.Height))
            {
                using (var g = new Context(bm))
                {
                    if (this.Background.IsVisible())
                    {
                        g.Save();
                        using (var pattern = new SolidPattern(this.Background.R, this.Background.G, this.Background.B, this.Background.A))
                        {
                            g.SetSource(pattern);
                            g.Rectangle(0, 0, this.Width, this.Height);
                            g.Fill();
                        }

                        g.Restore();
                    }

                    var rc = new GraphicsRenderContext { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    model.Update(true);
                    model.Render(rc, this.Width, this.Height);

                    // write to a temporary file
                    var tmp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid() + ".png");
                    bm.WriteToPng(tmp);
                    var bytes = File.ReadAllBytes(tmp);

                    // write to the stream
                    stream.Write(bytes, 0, bytes.Length);

                    // delete the temporary file
                    File.Delete(tmp);
                }
            }
        }
    }
}