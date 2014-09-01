// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataPointProvider.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality to provide a <see cref="DataPoint" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality to provide a <see cref="DataPoint" />.
    /// </summary>
    public interface IDataPointProvider
    {
        /// <summary>
        /// Gets the <see cref="DataPoint" /> that represents the element.
        /// </summary>
        /// <returns>A <see cref="DataPoint" />.</returns>
        DataPoint GetDataPoint();
    }
}