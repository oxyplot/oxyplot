using System.Windows;
using System.Windows.Controls;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Export a PlotModel to .png using WPF
    /// </summary>
    public static class PngExporter
    {
        public static void Export(PlotModel model, string fileName, int width, int height, OxyColor background = null)
        {
            var g = new Grid();
            if (background != null)
                g.Background = background.ToBrush();
            var p = new Plot { Model = model };
            g.Children.Add(p);

            var size = new Size(width, height);
            g.Measure(size);
            g.Arrange(new Rect(g.DesiredSize));
            g.UpdateLayout();

            p.SaveBitmap(fileName);
        }
    }
}