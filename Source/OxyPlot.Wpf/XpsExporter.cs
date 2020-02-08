// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XpsExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to xps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
#if !NET40
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Xps;
    using System.Windows.Xps.Packaging;

    /// <summary>
    /// Provides functionality to export plots to xps.
    /// </summary>
    public class XpsExporter : IExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XpsExporter" /> class.
        /// </summary>
        public XpsExporter()
        {
            this.Width = 600;
            this.Height = 400;
        }

        /// <summary>
        /// Gets or sets the width of the output document.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the output document.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the text formatting mode.
        /// </summary>
        /// <value>The text formatting mode.</value>
        public TextFormattingMode TextFormattingMode { get; set; }

        /// <summary>
        /// Exports the specified plot model to an xps file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void Export(IPlotModel model, string fileName, double width, double height)
        {
            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                var exporter = new XpsExporter { Width = width, Height = height };
                exporter.Export(model, stream);
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void Export(IPlotModel model, Stream stream, double width, double height)
        {
            var exporter = new XpsExporter { Width = width, Height = height };
            exporter.Export(model, stream);
        }

        /// <summary>
        /// Prints the specified plot model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (using the actual media width if set to NaN).</param>
        /// <param name="height">The height (using the actual media height if set to NaN).</param>
        public static void Print(IPlotModel model, double width, double height)
        {
            var exporter = new XpsExporter { Width = width, Height = height };
            exporter.Print(model);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to the specified <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        public void Export(IPlotModel model, Stream stream)
        {
            using (var xpsPackage = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var doc = new XpsDocument(xpsPackage))
                {
                    var xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
                    this.Write(model, xpsdw);
                }
            }
        }

        /// <summary>
        /// Prints the specified plot model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Print(IPlotModel model)
        {
            PrintDocumentImageableArea area = null;
            var xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsDocumentWriter != null)
            {
                if (double.IsNaN(this.Width))
                {
                    this.Width = area.ExtentWidth;
                }

                if (double.IsNaN(this.Height))
                {
                    this.Height = area.ExtentHeight;
                }

                this.Write(model, xpsDocumentWriter);
            }
        }

        /// <summary>
        /// Write the specified <see cref="IPlotModel" /> to the specified <see cref="XpsDocumentWriter" />.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="writer">The document writer.</param>
        private void Write(IPlotModel model, XpsDocumentWriter writer)
        {
            var canvas = new Canvas { Width = this.Width, Height = this.Height, Background = model.Background.ToBrush() };
            canvas.Measure(new Size(this.Width, this.Height));
            canvas.Arrange(new Rect(0, 0, this.Width, this.Height));

            var rc = new CanvasRenderContext(canvas);
            rc.TextFormattingMode = this.TextFormattingMode;

            model.Update(true);
            model.Render(rc, new OxyRect(0, 0, this.Width, this.Height));

            canvas.UpdateLayout();

            writer.Write(canvas);
        }
    }
#endif
}
