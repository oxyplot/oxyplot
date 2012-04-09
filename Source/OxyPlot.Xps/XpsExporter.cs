// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XpsExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Xps.Packaging;

    /// <summary>
    /// Exports or prints a PlotModel using xps.
    /// </summary>
    public static class XpsExporter
    {
        #region Public Methods

        /// <summary>
        /// Exports the specified plot model to an xps file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        public static void Export(PlotModel model, string fileName, double width, double height, OxyColor background = null)
        {
            using (var xpsPackage = Package.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var doc = new XpsDocument(xpsPackage))
                {
                    var canvas = new Canvas { Width = width, Height = height, Background = background.ToBrush() };
                    canvas.Measure(new Size(width, height));
                    canvas.Arrange(new Rect(0, 0, width, height));

                    var rc = new ShapesRenderContext(canvas);
                    model.Update();
                    model.Render(rc);

                    canvas.UpdateLayout();

                    var xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
                    xpsdw.Write(canvas);
                }
            }
        }

        /// <summary>
        /// Prints the specified plot model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (using the actual media width if not specified).</param>
        /// <param name="height">The height (using the actual media height if not specified).</param>
        public static void Print(PlotModel model, double width = double.NaN, double height = double.NaN)
        {
            PrintDocumentImageableArea area = null;
            var xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsDocumentWriter != null)
            {
                if (double.IsNaN(width))
                {
                    width = area.MediaSizeWidth;
                }

                if (double.IsNaN(height))
                {
                    height = area.MediaSizeHeight;
                }

                var canvas = new Canvas { Width = width, Height = height };
                canvas.Measure(new Size(width, height));
                canvas.Arrange(new Rect(0, 0, width, height));

                var rc = new ShapesRenderContext(canvas);
                model.Update();
                model.Render(rc);

                canvas.UpdateLayout();

                xpsDocumentWriter.Write(canvas);
            }
        }

        #endregion
    }
}