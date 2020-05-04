// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotBase.Export.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public partial class PlotView
    {
        /// <summary>
        /// Saves the PlotView as a bitmap.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveBitmap(string fileName)
        {
            this.SaveBitmap(fileName, -1, -1);
        }

        /// <summary>
        /// Saves the PlotView as a bitmap.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SaveBitmap(string fileName, int width, int height)
        {
            if (width <= 0)
            {
                width = (int)this.ActualWidth;
            }

            if (height <= 0)
            {
                height = (int)this.ActualHeight;
            }

            PngExporter.Export(this.ActualModel, fileName, width, height);
        }

        /// <summary>
        /// Saves the PlotView as xaml.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveXaml(string fileName)
        {
            XamlExporter.Export(this.ActualModel, fileName, this.ActualWidth, this.ActualHeight);
        }

        /// <summary>
        /// Renders the PlotView to xaml.
        /// </summary>
        /// <returns>The xaml.</returns>
        public string ToXaml()
        {
            return XamlExporter.ExportToString(this.ActualModel, this.ActualWidth, this.ActualHeight);
        }

        /// <summary>
        /// Renders the PlotView to a bitmap.
        /// </summary>
        /// <returns>A bitmap.</returns>
        public BitmapSource ToBitmap()
        {
            var exporter = new PngExporter() { Width = (int)this.ActualWidth, Height = (int)this.ActualHeight };
            return exporter.ExportToBitmap(this.ActualModel);
        }
    }
}
