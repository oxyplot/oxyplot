using System.IO;
using System.IO.Packaging;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;

namespace OxyPlot.Wpf
{
    /// <summary>
    /// Export or print a PlotModel using XPS
    /// </summary>
    public static class XpsExporter
    {
        public static void Export(PlotModel model, string fileName, double width, double height)
        {
            using (var xpsPackage = Package.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var doc = new XpsDocument(xpsPackage))
                {
                    var g = new Grid();
                    var p = new Plot { Model = model };
                    g.Children.Add(p);

                    // var size = new Size(area.MediaSizeWidth, area.MediaSizeHeight);
                    var size = new Size(width, height);
                    g.Measure(size);
                    g.Arrange(new Rect(size));
                    g.UpdateLayout();

                    var xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
                    if (xpsdw != null)
                    {
                        xpsdw.Write(g);
                    }

                }
            }
        }

        public static void Print(PlotModel model)
        {
            PrintDocumentImageableArea area = null;
            var xpsdw = PrintQueue.CreateXpsDocumentWriter(ref area);
            if (xpsdw != null)
            {
                var g = new Grid();
                var p = new Plot { Model = model };
                g.Children.Add(p);

                var size = new Size(area.MediaSizeWidth, area.MediaSizeHeight);
                g.Measure(size);
                g.Arrange(new Rect(size));
                g.UpdateLayout();

                xpsdw.Write(g);
            }
        }
    }
}