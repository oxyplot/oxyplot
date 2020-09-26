// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IXyAxisPlotElement.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.Axes;

    /// <summary>
    /// Defines a plot element that uses an X and a Y axis.
    /// </summary>
    public interface IXyAxisPlotElement : IPlotElement
    {
        /// <summary>
        /// Gets the X axis.
        /// </summary>
        Axis XAxis { get; }

        /// <summary>
        /// Gets the Y axis.
        /// </summary>
        Axis YAxis { get; }

        /// <summary>
        /// Transforms the specified data point to a screen point by the axes of the plot element.
        /// </summary>
        /// <param name="p">The data point.</param>
        /// <returns>A screen point.</returns>
        ScreenPoint Transform(DataPoint p);

        /// <summary>
        /// Transforms from a screen point to a data point by the axes of this series.
        /// </summary>
        /// <param name="p">The screen point.</param>
        /// <returns>A data point.</returns>
        DataPoint InverseTransform(ScreenPoint p);
    }
}
