// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
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
        #region Public Methods

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
            model.Render(rc);

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

        #endregion
    }
}