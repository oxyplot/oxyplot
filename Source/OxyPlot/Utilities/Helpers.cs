// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helpers.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Utilities
{
    /// <summary>
    /// Provides general helper functions.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Switches the values of two specified variables.
        /// </summary>
        /// <typeparam name="T">The type of the variables.</typeparam>
        /// <param name="value">The first value.</param>
        /// <param name="other">The second value.</param>
        public static void Swap<T>(ref T value, ref T other)
        {
            var tmp = value;
            value = other;
            other = tmp;
        }
    }
}
