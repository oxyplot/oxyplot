// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XpsExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.IO;
    using System.IO.Packaging;
    using System.Printing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Xps;
    using System.Windows.Xps.Packaging;

    /// <summary>
    /// Export or print a PlotModel using XPS
    /// </summary>
    public static class XpsExporter
    {
        #region Public Methods

        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public static void Export(PlotModel model, string fileName, double width, double height)
        {
            using (Package xpsPackage = Package.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
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

                    XpsDocumentWriter xpsdw = XpsDocument.CreateXpsDocumentWriter(doc);
                    if (xpsdw != null)
                    {
                        xpsdw.Write(g);
                    }
                }
            }
        }

        /// <summary>
        /// The print.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        public static void Print(PlotModel model)
        {
            PrintDocumentImageableArea area = null;
            XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(ref area);
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

        #endregion
    }
}