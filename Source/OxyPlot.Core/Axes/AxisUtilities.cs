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
    using System.Collections.Generic;

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

        /// <summary>
        /// Creates tick values at the specified interval.
        /// </summary>
        /// <param name="from">The start value.</param>
        /// <param name="to">The end value.</param>
        /// <param name="step">The interval.</param>
        /// <param name="maxTicks">The maximum number of ticks (optional). The default value is 1000.</param>
        /// <returns>A sequence of values.</returns>
        /// <exception cref="System.ArgumentException">Step cannot be zero or negative.;step</exception>
        public static IList<double> CreateTickValues(double from, double to, double step, int maxTicks = 1000)
        {
            if (step <= 0)
            {
                throw new ArgumentException("Step cannot be zero or negative.", "step");
            }

            if (to <= from && step > 0)
            {
                step *= -1;
            }

            var startValue = Math.Round(from / step) * step;
            var numberOfValues = Math.Max((int)((to - from) / step), 1);
            var epsilon = step * 1e-3 * Math.Sign(step);
            var values = new List<double>(numberOfValues);

            for (int k = 0; k < maxTicks; k++)
            {
                var lastValue = startValue + (step * k);

                // If we hit the maximum value before reaching the max number of ticks, exit
                if (lastValue > to + epsilon)
                {
                    break;
                }

                // try to get rid of numerical noise
                var v = Math.Round(lastValue / step, 14) * step;
                values.Add(v);
            }

            return values;
        }
    }
}
