using System.Windows;
using System.Windows.Controls;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Export a PlotModel to .png using WPF
    /// </summary>
    public static class PngExporter
    {
        public static void Export(PlotModel model, string fileName, int width, int height)
        {
            var g = new Grid();
            var p = new Plot { Model = model };
            g.Children.Add(p);

            var size = new Size(width, height);
            g.Measure(size);
            g.Arrange(new Rect(size));
            g.UpdateLayout();

            p.SaveBitmap(fileName);
        }
    }
}