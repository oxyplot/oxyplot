using System.IO;

namespace OxyPlot.Pdf
{
    /// <summary>
    /// Exporting PlotModels to PDF.
    /// </summary>
    public static class PdfExporter
    {
        /// <summary>
        /// Exports the specified model to a file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="path">The path.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(PlotModel model, string path, double width, double height)
        {
            using (var s = File.OpenWrite(path))
            {
                Export(model, s, width, height);
            }
        }

        /// <summary>
        /// Exports the specified model to a stream.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="s">The stream.</param>
        /// <param name="width">The width (points).</param>
        /// <param name="height">The height (points).</param>
        public static void Export(PlotModel model, Stream s, double width, double height)
        {
            var svgrc = new PdfRenderContext(width, height);
            model.UpdateData();
            model.Render(svgrc);
            svgrc.Save(s);
        }
    }
}