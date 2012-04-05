// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Export a PlotModel to .png using WPF
    /// </summary>
    public static class PngExporter
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
        /// <param name="background">
        /// The background.
        /// </param>
        public static void Export(PlotModel model, string fileName, int width, int height, OxyColor background = null)
        {
            var g = new Grid();
            if (background != null)
            {
                g.Background = background.ToBrush();
            }

            var p = new Plot { Model = model };
            g.Children.Add(p);


            var size = new Size(width, height);
            g.Measure(size);
            g.Arrange(new Rect(0, 0, width, height));
            p.RefreshPlot(true);
            g.UpdateLayout();

            p.SaveBitmap(fileName);
        }

        #endregion
    }
}