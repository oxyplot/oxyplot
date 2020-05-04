// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExporterExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides extension methods for exporters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.IO;

    /// <summary>
    /// Provides extension methods for exporters.
    /// </summary>
    public static class ExporterExtensions
    {
        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a file.
        /// </summary>
        /// <param name="exporter">The exporter.</param>
        /// <param name="model">The model to export.</param>
        /// <param name="path">The path to the file.</param>
        public static void ExportToFile(this IExporter exporter, IPlotModel model, string path)
        {
            using var stream = File.OpenWrite(path);
            exporter.Export(model, stream);
        }
    }
}
