// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XpsExporter.cs" company="OxyPlot">
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
    ///     Exports or prints a PlotModel to an xps file.
    /// </summary>
    public static class XpsExporter
    {
        /// <summary>
        ///     Exports the specified plot model to an xps file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background color.</param>
        public static void Export(
            PlotModel model, string fileName, double width, double height, OxyColor background = null)
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
        ///     Prints the specified plot model.
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
                model.Render(rc, width, height);

                canvas.UpdateLayout();

                xpsDocumentWriter.Write(canvas);
            }
        }
    }
}