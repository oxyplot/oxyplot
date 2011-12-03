// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringFormattingExtensions.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// String formatting extensions.
    /// </summary>
    public static class StringFormattingExtensions
    {
        #region Public Methods

        /// <summary>
        /// Formats a double to a 'nice' string.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string ToNiceString(this double x)
        {
            return x.ToString();
        }

        /// <summary>
        /// Formats a DateTime to a 'nice' string.
        /// </summary>
        /// <param name="x">
        /// The DateTime value.
        /// </param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string ToNiceString(this DateTime x)
        {
            return x.ToString();
        }

        /// <summary>
        /// Formats a TimeSpan to a 'nice' string.
        /// </summary>
        /// <param name="x">
        /// The TimeSpan value.
        /// </param>
        /// <returns>
        /// A string.
        /// </returns>
        public static string ToNiceString(this TimeSpan x)
        {
            return x.ToString();
        }

        #endregion
    }
}