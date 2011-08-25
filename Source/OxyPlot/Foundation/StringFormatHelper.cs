// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringFormatHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The formatting extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The formatting extensions.
    /// </summary>
    public static class FormattingExtensions
    {
        #region Public Methods

        /// <summary>
        /// The to nice string.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The to nice string.
        /// </returns>
        public static string ToNiceString(this double x)
        {
            return x.ToString();
        }

        /// <summary>
        /// The to nice string.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The to nice string.
        /// </returns>
        public static string ToNiceString(this DateTime x)
        {
            return x.ToString();
        }

        /// <summary>
        /// The to nice string.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <returns>
        /// The to nice string.
        /// </returns>
        public static string ToNiceString(this TimeSpan x)
        {
            return x.ToString();
        }

        #endregion
    }
}