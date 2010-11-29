using System.IO;

namespace OxyPlot.Pdf
{
    public static class PdfPlotWriter
    {
        public static void Save(PlotModel model, string path, double width, double height)
        {
            using (var s = File.OpenWrite(path))
            {
                Save(model, s, width, height);
            }
        }

        public static void Save(PlotModel model, Stream s, double width, double height)
        {
            var svgrc = new PdfRenderContext(width, height);
            model.Render(svgrc);
            svgrc.Save(s);
        }
    }
}