// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisUtilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Static utility methods for the Axis classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;

    /// <summary>
    /// Static utility methods for the <see cref="Axis" /> classes.
    /// </summary>
    public static class AxisUtilities
    {
        /// <summary>
        /// Calculates the minor interval.
        /// </summary>
        /// <param name="majorInterval">The major interval.</param>
        /// <returns>The minor interval.</returns>
        public static double CalculateMinorInterval(double majorInterval)
        {
            // check if majorInterval = 2*10^x
            // uses the mathematical identity log10(2 * 10^x) = x + log10(2)
            // -> we just have to check if the modulo of log10(2*10^x) = log10(2)
            if (Math.Abs(((Math.Log10(majorInterval) + 1000) % 1) - Math.Log10(2)) < 1e-10)
            {
                return majorInterval / 4;
            }

            return majorInterval / 5;
        }

#if DEBUG
        /// <summary>
        /// Calculates the minor interval (alternative algorithm).
        /// </summary>
        /// <param name="majorInterval">The major interval.</param>
        /// <returns>The minor interval.</returns>
        public static double CalculateMinorInterval2(double majorInterval)
        {
            var exponent = Math.Ceiling(Math.Log(majorInterval, 10));
            var mantissa = majorInterval / Math.Pow(10, exponent - 1);
            return (int)mantissa == 2 ? majorInterval / 4 : majorInterval / 5;
        }
#endif
    }
}
