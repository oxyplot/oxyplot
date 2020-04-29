// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExporter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines functionality to export a PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System.IO;

    /// <summary>
    /// Defines functionality to export a <see cref="PlotModel" />.
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// Exports the specified <see cref="PlotModel" /> to a <see cref="Stream" />.
        /// </summary>
        /// <param name="model">The model to export.</param>
        /// <param name="stream">The target stream.</param>
        void Export(IPlotModel model, Stream stream);
    }
}