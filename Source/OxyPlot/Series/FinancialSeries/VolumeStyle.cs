// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeStyle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents rendering style for volume in either CandleStickAndVolumeSeries or VolumeSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    /// <summary>
    /// Represents rendering style for volume in either <see cref="CandleStickAndVolumeSeries" /> or <see cref="VolumeSeries" />.
    /// </summary>
    public enum VolumeStyle
    {
        /// <summary>
        /// Volume is not displayed
        /// </summary>
        None,

        /// <summary>
        /// Buy + Sell volume summed to produce net positive or negative volume
        /// </summary>
        Combined,

        /// <summary>
        /// Buy and Sell volume is stacked, one on top of the other, with the dominant on top
        /// </summary>
        Stacked,

        /// <summary>
        /// Buy volume above y=0 axis and Sell volume below y=0 axis
        /// </summary>
        PositiveNegative
    }
}
