// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisExtensions.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Extensions methods for the Axis class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;

    /// <summary>
    /// Extension methods for the Axis class.
    /// </summary>
    public static class AxisExtensions
    {
        /// <summary>
        /// Calculates whether snapping is required and what the new values should be.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <returns>The snapping calculation result.</returns>
        public static Tuple<bool, double, double> CalculateSnapping(this Axis axis)
        {
            // We must "snap" to valid values in order to make our axis correct
            var precision = Axis.GetPrecision(axis.ActualMaximum - axis.ActualMinimum);

            var newActualMinimum = Math.Round(axis.ActualMinimum, precision);
            var newActualMaximum = Math.Round(axis.ActualMaximum, precision);
            var hasChanges = false;

            if (axis.ActualMinimum != newActualMinimum)
            {
                hasChanges = true;
            }

            if (axis.ActualMaximum != newActualMaximum)
            {
                hasChanges = true;
            }

            return new Tuple<bool, double, double>(hasChanges, newActualMinimum, newActualMaximum);
        }
    }
}