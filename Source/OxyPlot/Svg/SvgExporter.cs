// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.IO;

    /// <summary>
    /// Exports plot models to svg.
    /// </summary>
    public static class SvgExporter
    {
        #region Public Methods

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="stream">The output stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="textMeasurer">The text measurer.</param>
        public static void Export(PlotModel model, Stream stream, double width, double height, IRenderContext textMeasurer = null)
        {
            using (var svgrc = new SvgRenderContext(stream, width, height, true, textMeasurer))
            {
                model.Update();
                model.Render(svgrc);
            }
        }

#if !PCL
        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="textMeasurer">The text measurer.</param>
        public static void Export(PlotModel model, string fileName, double width, double height, IRenderContext textMeasurer = null)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height, textMeasurer))
            {
                model.Update();
                model.Render(svgrc);
            }
        }
#endif

        /// <summary>
        /// Exports to string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        /// <param name="textMeasurer">The text measurer.</param>
        /// <param name="isDocument">if set to <c>true</c>, the xml headers will be included (?xml and !DOCTYPE).</param>
        /// <returns>
        /// The plot as a svg string.
        /// </returns>
        public static string ExportToString(PlotModel model, double width, double height, bool isDocument = false, IRenderContext textMeasurer = null)
        {
            string svg;
            using (var ms = new MemoryStream())
            {
                var svgrc = new SvgRenderContext(ms, width, height, isDocument, textMeasurer);
                model.Update();
                model.Render(svgrc);
                svgrc.Complete();
                svgrc.Flush();
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                svg = sr.ReadToEnd();
            }

            return svg;
        }

        #endregion
    }
}