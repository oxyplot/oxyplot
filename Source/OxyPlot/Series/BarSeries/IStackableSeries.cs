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
    public interface IStackableSeries
    {
        /// <summary>
        /// Gets a value indicating whether this series is stacked.
        /// </summary>
        bool IsStacked { get; }

        /// <summary>
        /// Gets the stack group.
        /// </summary>
        /// <value>The stack group.</value>
        string StackGroup { get; }
    }
}