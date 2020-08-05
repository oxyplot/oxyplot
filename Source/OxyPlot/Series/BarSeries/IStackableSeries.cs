// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStackableSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines properties for stacked series.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Defines properties for stacked series.
    /// </summary>
    public interface IStackableSeries : IBarSeries
    {
        /// <summary>
        /// Gets a value indicating whether this series is stacked.
        /// </summary>
        bool IsStacked { get; }

        /// <summary>
        /// Gets a value indicating whether this series should overlap its stack when <see cref="IsStacked"/> is true.
        /// </summary>
        bool OverlapsStack { get; }

        /// <summary>
        /// Gets the stack group.
        /// </summary>
        /// <value>The stack group.</value>
        string StackGroup { get; }
    }
}
