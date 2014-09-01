// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisUtilities.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides utility methods for axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides utility methods for axes.
    /// </summary>
    public static class AxisUtilities
    {
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

            var value = Math.Round(from / step) * step;
            var numberOfValues = Math.Max((int)((to - from) / step), 1);
            var epsilon = step * 1e-3 * Math.Sign(step);
            var values = new List<double>(numberOfValues);
            var i = 0;

            while (value <= to + epsilon && i < maxTicks)
            {
                i++;

                // try to get rid of numerical noise
                var v = Math.Round(value / step, 14) * step;
                values.Add(v);
                value += step;
            }

            return values;
        }
    }
}