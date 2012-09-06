// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Export a PlotModel to .png using WPF
    /// </summary>
    public static class PngExporter
    {
        #region Public Methods

        /// <summary>
        /// Exports the specified plot model to a file.
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
        public static void Export(PlotModel model, string fileName, int width, int height, OxyColor background = null)
        {
            var bmp = ExportToBitmap(model, width, height, background);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            using (var s = File.Create(fileName))
            {
                encoder.Save(s);
            }
        }

        /// <summary>
        /// Exports the specified plot model to a bitmap.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <param name="dpi">The resolution.</param>
        /// <returns>A bitmap.</returns>
        public static BitmapSource ExportToBitmap(PlotModel model, int width, int height, OxyColor background = null, int dpi = 96)
        {
            var canvas = new Canvas { Width = width, Height = height, Background = background.ToBrush() };
            canvas.Measure(new Size(width, height));
            canvas.Arrange(new Rect(0, 0, width, height));

            var rc = new ShapesRenderContext(canvas);
            model.Update();
            model.Render(rc);

            canvas.UpdateLayout();

            var bmp = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Pbgra32);
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
        #endregion
    }
}