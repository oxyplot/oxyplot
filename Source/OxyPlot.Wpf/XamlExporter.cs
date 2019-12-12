// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to export plots to XAML.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;

    /// <summary>
    /// Provides functionality to export plots to XAML.
    /// </summary>
    public static class XamlExporter
    {
        /// <summary>
        /// Export the specified plot model to an xaml string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A xaml string.</returns>
        public static string ExportToString(IPlotModel model, double width, double height)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });
                Export(model, xw, width, height);
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
        public static void Export(PlotModel model, string fileName, double width, double height)
        {
            using (var sw = new StreamWriter(fileName))
            {
                var xw = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });
                Export(model, xw, width, height);
            }
        }

        /// <summary>
        /// Exports the specified plot model to a xml writer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="writer">The xml writer.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        private static void Export(IPlotModel model, XmlWriter writer, double width, double height)
        {
            var c = new Canvas();
            if (model.Background.IsVisible())
            {
                c.Background = model.Background.ToBrush();
            }

            c.Measure(new Size(width, height));
            c.Arrange(new Rect(0, 0, width, height));

            var rc = new CanvasRenderContext(c) { UseStreamGeometry = false };

            rc.TextFormattingMode = TextFormattingMode.Ideal;

            model.Update(true);
            model.Render(rc, new OxyRect(0, 0, width, height));

            c.UpdateLayout();

            XamlWriter.Save(c, writer);
        }
    }
}
