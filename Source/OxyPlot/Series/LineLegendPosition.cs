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
        /// Render legend at the end of the line.
        /// </summary>
        End
    }
}