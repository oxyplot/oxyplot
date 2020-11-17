// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisPosition.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the position of an <see cref="Axis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Specifies the position of an <see cref="Axis" />.
    /// </summary>
    public enum AxisPosition
    {
        /// <summary>
        /// No position.
        /// </summary>
        None,

        /// <summary>
        /// Left of the plot area.
        /// </summary>
        Left,

        /// <summary>
        /// Right of the plot area.
        /// </summary>
        Right,

        /// <summary>
        /// Top of the plot area.
        /// </summary>
        Top,

        /// <summary>
        /// Bottom of the plot area.
        /// </summary>
        Bottom,

        /// <summary>
        /// All positions.
        /// </summary>
        All,
    }
}
