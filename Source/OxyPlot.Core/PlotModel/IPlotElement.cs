// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlotElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality for an element of a plot.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Specifies functionality for an element of a plot.
    /// </summary>
    public interface IPlotElement
    {
        /// <summary>
        /// Returns a hash code for this element.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        /// <remarks>This method creates the hash code by reflecting the value of all public properties.</remarks>
        int GetElementHashCode();
    }
}