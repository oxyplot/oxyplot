using System.Drawing;
using System.IO;

// --------------------------------------------------------------------------------------------------------------------
// <copyright company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The png exporter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Core.Drawing
{
    /// <summary>
    /// The png exporter.
    /// </summary>
    public class PngExporter
    {
        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="background">
        /// The background.
        /// </param>
        public static void Export(IPlotModel model, string fileName, int width, int height, Brush background = null)
        {
            using (var bm = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    if (background != null)
                    {
                        g.FillRectangle(background, 0, 0, width, height);
                    }

                    var rc = new GraphicsRenderContext(g) { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    model.Update(true);
                    model.Render(rc, width, height);
                    bm.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

       /// <summary>
       /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
       /// </summary>
       /// <param name="model">The model.</param>
       /// <param name="stream">The output stream.</param>
       /// <param name="width"></param>
       /// <param name="height"></param>
       /// <param name="background"></param>
       /// <param name="resolution"></param>
       public static void Export(IPlotModel model, Stream stream, int width, int height, OxyColor background, float resolution)
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
       /// <param name="stream">The output stream.</param>
       /// <param name="width"></param>
       /// <param name="height"></param>
       /// <param name="background"></param>
       /// <param name="resolution"></param>
       public static Stream ExportToStream(IPlotModel model, int width, int height, OxyColor background, float resolution = 96)
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
       /// <param name="model">The model to export.</param>
       /// <param name="width"></param>
       /// <param name="height"></param>
       /// <param name="background"></param>
       /// <param name="resolution"></param>
       /// <returns>A bitmap.</returns>
       public static Bitmap ExportToBitmap(IPlotModel model, int width, int height, OxyColor background, float resolution)
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

             bm.SetResolution(resolution, resolution);
             return bm;
          }
       }

       public PngExporter()
       {
       }

       public int Width { get; set; }
       public int Height { get; set; }
       public OxyColor Background { get; set; }
       public float Resolution { get; set; }

       public void Export(IPlotModel model, Stream stream) => Export(model, stream, Width, Height, OxyColors.White, Resolution);
       public void ExportToFile(IPlotModel model, string filename) => Export(model, filename, Width, Height, Background.ToBrush());
    }
}
