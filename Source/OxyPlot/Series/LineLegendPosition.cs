// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineLegendPosition.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the position of legends rendered on a <see cref="LineSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Specifies the position of legends rendered on a <see cref="LineSeries" />.
    /// </summary>
    public enum LineLegendPosition
    {
        /// <summary>
        /// Do not render legend on the line.
        /// </summary>
        None,

        /// <summary>
        /// Render legend at the start of the line.
        /// </summary>
        Start,

        /// <summary>
        /// Render legend at the start of the line inside the plot area.
        /// </summary>
        StartInside,

        /// <summary>
        /// Render legend at the start of the line inside the plot area.
        /// </summary>
        StartInsideBelow,

        /// <summary>
        /// Render legend above the midpoint of the line inside the plot area.
        /// </summary>
        HalfWayAbove,

        /// <summary>
        /// Render legend above the midpoint of the line inside the plot area. Text is right justified.
        /// </summary>
        HalfWayAboveRight,

        /// <summary>
        /// Render legend above the midpoint of the line inside the plot area. Text is left justified.
        /// </summary>
        HalfWayAboveLeft,

        /// <summary>
        /// Render legend above the midpoint of the line inside the plot area. This will follow the slope of the line
        /// </summary>
        HalfWayAboveSloped,

        /// <summary>
        /// Render legend below the midpoint of the line inside the plot area.
        /// </summary>
        HalfWayBelow,

        /// <summary>
        /// Render legend below the midpoint of the line inside the plot area. Text is right justified.
        /// </summary>
        HalfWayBelowRight,

        /// <summary>
        /// Render legend below the midpoint of the line inside the plot area. Text is left justified.
        /// </summary>
        HalfWayBelowLeft,
        /// <summary>
        /// Render legend below the midpoint of the line inside the plot area. This will follow the slope of the line.
        /// </summary>
        HalfWayBelowSloped,
        /// <summary>
        /// Render legend at the end of the line.
        /// </summary>
        End
    }
}
