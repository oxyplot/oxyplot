using System.Drawing;
using System.IO;
using NUnit.Framework;
using OxyPlot.Wpf;

namespace OxyPlot.Tests
{
    public static class OxyAssert
    {
        /// <summary>
        /// Asserts that a plot is equal to the plot stored in the "baseline" folder.
        /// 1. Renders the plot to file.png
        /// 2. If the baseline does not exist, the current plot is copied to the baseline folder.
        /// 3. Checks that the png file is equal to a baseline png.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="plot">The plot.</param>
        public static void AreEqual(PlotModel plot)
        {
            string name = new System.Diagnostics.StackFrame(1).GetMethod().Name;
            string path = name + ".png";
            string baseline = @"baseline\" + path;
            PngExporter.Export(plot, path, 800, 500, OxyColors.White);

            if (!Directory.Exists("baseline"))
                Directory.CreateDirectory("baseline");
            if (!File.Exists(baseline))
            {
                File.Copy(path, baseline);
            }

            var baselineImage = new Bitmap(baseline);
            var plotImage = new Bitmap(path);
            Assert.AreEqual(baselineImage.Width, plotImage.Width, "Image width");
            Assert.AreEqual(baselineImage.Height, plotImage.Height, "Image height");

            for (int x = 0; x < baselineImage.Width; x++)
                for (int y = 0; y < baselineImage.Height; y++)
                    Assert.AreEqual(baselineImage.GetPixel(x, y), plotImage.GetPixel(x, y),
                                    string.Format("Pixel ({0},{1})", x, y));
        }
    }
}