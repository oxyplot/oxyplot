// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlExporter.cs" company="OxyPlot">
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
//   Export a PlotModel to .xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Xml;

    /// <summary>
    /// Export a PlotModel to .xaml
    /// </summary>
    public static class XamlExporter
    {
        /// <summary>
        /// Export the specified plot model to an xaml string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        /// <returns>A xaml string.</returns>
        public static string ExportToString(PlotModel model, double width, double height, OxyColor background = null)
        {
            var g = new Grid();
            if (background != null)
            {
                g.Background = background.ToBrush();
            }

            var c = new Canvas();
            g.Children.Add(c);

            var size = new Size(width, height);
            g.Measure(size);
            g.Arrange(new Rect(0, 0, width, height));
            g.UpdateLayout();

            var rc = new ShapesRenderContext(c) { UseStreamGeometry = false };
            model.Update();
            model.Render(rc, width, height);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });
                XamlWriter.Save(c, xw);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exports the specified plot model to a xaml file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        public static void Export(PlotModel model, string fileName, double width, double height, OxyColor background = null)
        {
            using (var w = new StreamWriter(fileName))
            {
                w.Write(ExportToString(model, width, height, background));
            }
        }

    }
}