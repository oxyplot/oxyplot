// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotLengthUnit.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the kind of value that a <see cref="PlotLength" /> object is holding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines the kind of value that a <see cref="PlotLength" /> object is holding.
    /// </summary>
    public enum PlotLengthUnit
    {
        /// <summary>
        /// The value is in data space (transformed by x/y axis)
        /// </summary>
        Data = 0,

        /// <summary>
        /// The value is in screen units
        /// </summary>
        ScreenUnits = 1,

        /// <summary>
        /// The value is relative to the plot viewport (0-1)
        /// </summary>
        RelativeToViewport = 2,

        /// <summary>
        /// The value is relative to the plot area (0-1)
        /// </summary>
        RelativeToPlotArea = 3
    }
}