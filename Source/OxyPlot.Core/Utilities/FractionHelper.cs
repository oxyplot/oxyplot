// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FractionHelper.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to generate fraction strings from double values.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Provides functionality to generate fraction strings from double values.
    /// </summary>
    /// <remarks>Examples: "3/4", "PI/2"</remarks>
    public static class FractionHelper
    {
        /// <summary>
        /// Converts a double to a fraction string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="unitSymbol">The unit symbol.</param>
        /// <param name="eps">The tolerance.</param>
        /// <param name="formatProvider">The format Provider.</param>
        /// <param name="formatString">The format string.</param>
        /// <returns>The convert to fraction string.</returns>
        public static string ConvertToFractionString(
            double value,
            double unit = 1,
            string unitSymbol = null,
            double eps = 1e-6,
            IFormatProvider formatProvider = null,
            string formatString = null)
        {
            if (Math.Abs(value) < eps)
            {
                return "0";
            }

            // ½, ⅝, ¾
            value /= unit;

            // int whole = (int)(value - (int) value);
            // int N = 10000;
            // int frac = (int) ((value - whole)*N);
            // var d = GCF(N,frac);
            for (int d = 1; d <= 64; d++)
            {
                double n = value * d;
                var ni = (int)Math.Round(n);
                if (Math.Abs(n - ni) < eps)
                {
                    string nis = unitSymbol == null || ni != 1 ? ni.ToString(CultureInfo.InvariantCulture) : string.Empty;
                    if (d == 1)
                    {
                        return string.Format("{0}{1}", nis, unitSymbol);
                    }

                    return string.Format("{0}{1}/{2}", nis, unitSymbol, d);
                }
            }

            var format = string.IsNullOrEmpty(formatString) ? "{0}{1}" : "{0:" + formatString + "}{1}";
            return string.Format(formatProvider ?? CultureInfo.CurrentCulture, format, value, unitSymbol);
        }
    }
}