using System;

namespace OxyPlot
{
    /// <summary>
    /// Describes the Interface for a shape.
    /// </summary>
    public interface IShape
    {
        /// <summary>
        /// Determines whether the specified point is inside the shape.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns><c>true</c> if the shape contains the specified point; otherwise, <c>false</c>.</returns>
        bool Contains(double x, double y);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString();

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider);


    }
}
