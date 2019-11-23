// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITransposablePlotElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The TransposablePlotElement interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.Axes;

    /// <summary>
    /// The TransposablePlotElement interface.
    /// </summary>
    public interface ITransposablePlotElement : IPlotElement
    {
        /// <summary>
        /// Gets the X axis.
        /// </summary>
        Axis XAxis { get; }

        /// <summary>
        /// Gets the Y axis.
        /// </summary>
        Axis YAxis { get; }
    }
}
