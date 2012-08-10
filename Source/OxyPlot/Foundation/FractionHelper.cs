// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FractionHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Generates fraction strings from double values.
    /// </summary>
    /// <remarks>
    /// Examples: "3/4", "PI/2"
    /// </remarks>
    public class FractionHelper
    {
        #region Public Methods

        /// <summary>
        /// Converts a double to a fraction string.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="unit">
        /// The unit.
        /// </param>
        /// <param name="unitSymbol">
        /// The unit symbol.
        /// </param>
        /// <param name="eps">
        /// The tolerance.
        /// </param>
        /// <param name="formatProvider">
        /// The format Provider.
        /// </param>
        /// <returns>
        /// The convert to fraction string.
        /// </returns>
        public static string ConvertToFractionString(
            double value, 
            double unit = 1, 
            string unitSymbol = null, 
            double eps = 1e-6, 
            IFormatProvider formatProvider = null)
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

            return string.Format(formatProvider ?? CultureInfo.CurrentCulture, "{0}{1}", value, unitSymbol);
        }

        /// <summary>
        /// The gcd.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The gcd.
        /// </returns>
        public static int gcd(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }

            return gcd(b, a % b);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The gcf.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The gcf.
        /// </returns>
        private static int GCF(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            int z;
            do
            {
                z = x % y;
                if (z == 0)
                {
                    return y;
                }

                x = y;
                y = z;
            }
            while (true);
        }

        #endregion
    }
}