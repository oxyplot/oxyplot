using System;

namespace OxyPlot
{
    /// <summary>
    /// Describes the Interface for a shape.
    /// </summary>
    public interface IOxyRegion
    {
        /// <summary>
        /// Determines whether the specified point is inside the shape.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the shape contains the specified point; otherwise, <c>false</c>.</returns>
        bool Contains(double x, double y);
    }
}
