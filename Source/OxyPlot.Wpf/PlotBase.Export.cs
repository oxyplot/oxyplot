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
    public partial class PlotBase
    {
        /// <summary>
        /// Saves the PlotView as a bitmap.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveBitmap(string fileName)
        {
            this.SaveBitmap(fileName, -1, -1, this.ActualModel.Background);
        }

        /// <summary>
        /// Saves the PlotView as a bitmap.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        public void SaveBitmap(string fileName, int width, int height, OxyColor background)
        {
            if (width <= 0)
            {
                width = (int)this.ActualWidth;
            }

            if (height <= 0)
            {
                height = (int)this.ActualHeight;
            }

            if (!background.IsVisible())
            {
                background = this.Background.ToOxyColor();
            }

            PngExporter.Export(this.ActualModel, fileName, width, height, background);
        }

        /// <summary>
        /// Saves the PlotView as xaml.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void SaveXaml(string fileName)
        {
            var background = this.ActualModel.Background.IsVisible() ? this.ActualModel.Background : this.Background.ToOxyColor();
            XamlExporter.Export(this.ActualModel, fileName, this.ActualWidth, this.ActualHeight, background);
        }

        /// <summary>
        /// Renders the PlotView to xaml.
        /// </summary>
        /// <returns>The xaml.</returns>
        public string ToXaml()
        {
            var background = this.ActualModel.Background.IsVisible() ? this.ActualModel.Background : this.Background.ToOxyColor();
            return XamlExporter.ExportToString(this.ActualModel, this.ActualWidth, this.ActualHeight, background);
        }

        /// <summary>
        /// Renders the PlotView to a bitmap.
        /// </summary>
        /// <returns>A bitmap.</returns>
        public BitmapSource ToBitmap()
        {
            var background = this.ActualModel.Background.IsVisible() ? this.ActualModel.Background : this.Background.ToOxyColor();
            return PngExporter.ExportToBitmap(this.ActualModel, (int)this.ActualWidth, (int)this.ActualHeight, background);
        }
    }
}
