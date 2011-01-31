using System.Drawing;
using System.Drawing.Imaging;
using Oxyplot.WindowsForms;

namespace OxyPlot.WindowsForms
{
    public static class PngExporter
    {
        public static void Export(PlotModel model, string fileName, int width, int height, Brush background = null)
        {
            using (var bm = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(bm))
                {
                    if (background != null)
                    {
                        g.FillRectangle(background, 0, 0, width, height);
                    }

                    var rc = new GraphicsRenderContext(g, width, height);

                    model.UpdateData();
                    model.Render(rc);
                    bm.Save(fileName, ImageFormat.Png);
                }
            }
        }
    }
}