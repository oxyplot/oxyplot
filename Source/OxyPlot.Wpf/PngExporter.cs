using System.Windows;
using System.Windows.Controls;

namespace OxyPlot.Wpf
{
    public static class PngExporter
    {
        public static void Export(PlotModel model, string fileName, int width, int height)
        {
            var g = new Grid();
            var p = new Plot {Model = model};
            g.Children.Add(p);

            var size = new Size(width, height + 1);
            // todo: something wrong here, shouldn't be neccessary to measure twice...
            g.Measure(size);
            g.Arrange(new Rect(size));
            p.Refresh();

            size = new Size(width, height);
            g.Measure(size);
            g.Arrange(new Rect(size));

            p.SaveBitmap(fileName);
        }
    }
}