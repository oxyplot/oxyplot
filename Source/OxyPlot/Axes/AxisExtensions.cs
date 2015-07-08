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
    using System.Diagnostics;

    /// <summary>
    /// Extension methods for the Axis class.
    /// </summary>
    public static class AxisExtensions
    {
        /// <summary>
        /// Snaps the specified axis to values that actually make sense.
        /// </summary>
        /// <param name="axis">The axis.</param>
        public static void Snap(this Axis axis)
        {
            var snapping = axis.CalculateSnapping();
            if (snapping.Item1)
            {
                Debug.WriteLine("Snapping actual minimum from '{0}' to '{1}'", axis.ActualMinimum, snapping.Item2);
                Debug.WriteLine("Snapping actual maximum from '{0}' to '{1}'", axis.ActualMaximum, snapping.Item3);

                axis.UpdateActualMaxMin(snapping.Item2, snapping.Item3);
            }
        }

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