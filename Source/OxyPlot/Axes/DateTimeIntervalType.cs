// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeIntervalType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the interval for a <see cref="DateTimeAxis" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    /// <summary>
    /// Specifies the interval for a <see cref="DateTimeAxis" />.
    /// </summary>
    public enum DateTimeIntervalType
    {
        /// <summary>
        /// Automatically determine interval.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Manual definition of intervals.
        /// </summary>
        Manual = 1,

        /// <summary>
        /// Interval type is milliseconds.
        /// </summary>
        Milliseconds = 2,

        /// <summary>
        /// Interval type is seconds.
        /// </summary>
        Seconds = 3,

        /// <summary>
        /// Interval type is minutes.
        /// </summary>
        Minutes = 4,

        /// <summary>
        /// Interval type is hours.
        /// </summary>
        Hours = 5,

        /// <summary>
        /// Interval type is days.
        /// </summary>
        Days = 6,

        /// <summary>
        /// Interval type is weeks.
        /// </summary>
        Weeks = 7,

        /// <summary>
        /// Interval type is months.
        /// </summary>
        Months = 8,

        /// <summary>
        /// Interval type is years.
        /// </summary>
        Years = 9,
    }
}