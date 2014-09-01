// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScatterPointProvider.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines functionality to provide a <see cref="ScatterPoint" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Defines functionality to provide a <see cref="ScatterPoint" />.
    /// </summary>
    public interface IScatterPointProvider
    {
        /// <summary>
        /// Gets the <see cref="ScatterPoint" /> that represents the element.
        /// </summary>
        /// <returns>A <see cref="ScatterPoint" />.</returns>
        ScatterPoint GetScatterPoint();
    }
}