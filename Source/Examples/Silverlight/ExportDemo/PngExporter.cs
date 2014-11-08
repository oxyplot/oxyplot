// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PngExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Exports a PlotModel to .png using ImageTools
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExportDemo
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using ImageTools;

    using OxyPlot;
    using OxyPlot.Silverlight;

    /// <summary>
    /// Exports a PlotModel to .png using ImageTools
    /// </summary>
    public static class PngExporter
    {
        /// <summary>
        /// Exports the specified plot model to a stream.
        /// </summary>
        /// <param name="model">The plot model.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="width">The width of the export image.</param>
        /// <param name="height">The height of the exported image.</param>
        /// <param name="background">The background.</param>
        public static void Export(IPlotModel model, Stream stream, double width, double height, OxyColor background)
        {
            var canvas = new Canvas { Width = width, Height = height };
            if (background.IsVisible())
            {
                canvas.Background = background.ToBrush();
            }

            canvas.Measure(new Size(width, height));
            canvas.Arrange(new Rect(0, 0, width, height));

            var rc = new CanvasRenderContext(canvas);
            model.Update(true);
            model.Render(rc, width, height);

            canvas.UpdateLayout();
            var image = canvas.ToImage();
            image.WriteToStream(stream);
        }
    }
}