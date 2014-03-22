// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XpsExporter.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Provides functionality to export plots to xps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xps
{
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Xps.Packaging;

    using OxyPlot.Wpf;

    /// <summary>
    /// Provides functionality to export plots to xps.
    /// </summary>
    public class XpsExporter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XpsExporter"/> class.
        /// </summary>
        public XpsExporter()
        {
            this.Width = 600;
            this.Height = 400;
            this.Background = OxyColors.White;
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
        /// Gets or sets the background color.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        ///     Exports the specified plot model to an xps file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        public static void Export(PlotModel model, string fileName, double width, double height, OxyColor background)
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
                    model.Render(rc, width, height);

                    canvas.UpdateLayout();

                    var xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
                    xpsdw.Write(canvas);
                }
            }
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public static void Export(PlotModel model, Stream stream, double width, double height)
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
        public static void Print(PlotModel model, double width, double height)
        {
            var exporter = new XpsExporter { Width = width, Height = height, Background = model.Background };
            exporter.Print(model);
        }

        /// <summary>
        /// Exports the specified <see cref="PlotModel"/> to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The stream.</param>
        public void Export(PlotModel model, Stream stream)
        {
            using (var xpsPackage = Package.Open(stream))
            {
                using (var doc = new XpsDocument(xpsPackage))
                {
                    var canvas = new Canvas { Width = this.Width, Height = this.Height, Background = this.Background.ToBrush() };
                    canvas.Measure(new Size(this.Width, this.Height));
                    canvas.Arrange(new Rect(0, 0, this.Width, this.Height));

                    var rc = new ShapesRenderContext(canvas);
                    model.Update();
                    model.Render(rc, this.Width, this.Height);

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
        public void Print(PlotModel model)
        {
            PrintDocumentImageableArea area = null;
            var xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsDocumentWriter != null)
            {
                var width = this.Width;
                var height = this.Height;
                if (double.IsNaN(width))
                {
                    width = area.MediaSizeWidth;
                }

                if (double.IsNaN(height))
                {
                    height = area.MediaSizeHeight;
                }

                var canvas = new Canvas { Width = width, Height = height, Background = this.Background.ToBrush() };
                canvas.Measure(new Size(width, height));
                canvas.Arrange(new Rect(0, 0, width, height));

                var rc = new ShapesRenderContext(canvas);
                model.Update();
                model.Render(rc, width, height);

                canvas.UpdateLayout();

                xpsDocumentWriter.Write(canvas);
            }
        }
    }
}