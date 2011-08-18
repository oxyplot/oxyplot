// -----------------------------------------------------------------------
// <copyright file="FractionHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Generate fraction strings from doubles.
    /// e.g. 0.75 => "3/4"
    /// e.g. 1.57.. => "PI/2"
    /// </summary>
    public class FractionHelper
    {
        public static string ConvertToFractionString(double value, double unit = 1, string unitSymbol = null, double eps = 1e-6)
        {
            if (Math.Abs(value) < eps) return "0";

            // ½, ⅝, ¾
            value /= unit;

            //int whole = (int)(value - (int) value);
            //int N = 10000;
            //int frac = (int) ((value - whole)*N);
            //var d = GCF(N,frac);

            for (int d = 1; d <= 64; d++)
            {
                double n = value * d;
                int ni = (int)Math.Round(n);
                if (Math.Abs(n - ni) < eps)
                {
                    var nis = (unitSymbol == null || ni != 1 ? ni.ToString() : "");
                    if (d == 1)
                        return String.Format("{0}{1}", nis, unitSymbol);
                    else
                        return String.Format("{0}{1}/{2}", nis, unitSymbol, d);
                }
            }
            return String.Format(CultureInfo.InvariantCulture, "{0}{1}", value, unitSymbol);
        }

        static int GCF(int x, int y)
        {
            x = Math.Abs(x);
            y = Math.Abs(y);
            int z;
            do
            {
                z = x % y;
                if (z == 0)
                    return y;
                x = y;
                y = z;
            } while (true);
        }

        public static int gcd(int a, int b)
        {
            if (b == 0)
                return a;
            return gcd(b, a % b);
        }
    }
}
