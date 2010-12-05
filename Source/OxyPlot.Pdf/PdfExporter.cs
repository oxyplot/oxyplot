using System.IO;

namespace OxyPlot.Pdf
{
    public static class PdfExporter
    {
        public static void Export(PlotModel model, string path, double width, double height)
        {
            using (var s = File.OpenWrite(path))
            {
                Export(model, s, width, height);
            }
        }

        public static void Export(PlotModel model, Stream s, double width, double height)
        {
            var svgrc = new PdfRenderContext(width, height);
            model.Render(svgrc);
            svgrc.Save(s);
        }
    }
}