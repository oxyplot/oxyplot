// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighLowItemGenerator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Creates realistic high/low items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Creates realistic high/low items.
    /// </summary>
    public static class HighLowItemGenerator
    {
        /// <summary>
        /// The random number generator.
        /// </summary>
        private static readonly Random Rand = new Random();

        /// <summary>
        /// Creates bars governed by a MR process
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="n">N.</param>
        /// <param name="x0">X0.</param>
        /// <param name="csigma">Csigma.</param>
        /// <param name="esigma">Esigma.</param>
        /// <param name="kappa">Kappa.</param>
        public static IEnumerable<HighLowItem> MRProcess(
            int n,
            double x0 = 100.0,
            double csigma = 0.50,
            double esigma = 0.70,
            double kappa = 0.01)
        {
            double x = x0;

            var baseT = DateTime.UtcNow;
            for (int ti = 0; ti < n; ti++)
            {
                var dx_c = -kappa * (x - x0) + RandomNormal(0, csigma);
                var dx_1 = -kappa * (x - x0) + RandomNormal(0, esigma);
                var dx_2 = -kappa * (x - x0) + RandomNormal(0, esigma);

                var open = x;
                var close = x = x + dx_c;
                var low = Min(open, close, open + dx_1, open + dx_2);
                var high = Max(open, close, open + dx_1, open + dx_2);

                var nowT = baseT.AddSeconds(ti);
                var t = DateTimeAxis.ToDouble(nowT);
                yield return new HighLowItem(t, high, low, open, close);
            }
        }

        /// <summary>
        /// Finds the minimum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
        /// <returns>The minimum.</returns>
        private static double Min(double a, double b, double c, double d)
        {
            return Math.Min(a, Math.Min(b, Math.Min(c, d)));
        }

        /// <summary>
        /// Finds the maximum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
        /// <returns>The maximum.</returns>
        private static double Max(double a, double b, double c, double d)
        {
            return Math.Max(a, Math.Max(b, Math.Max(c, d)));
        }

        /// <summary>
        /// Get random normal
        /// </summary>
        /// <param name="mu">Mu.</param>
        /// <param name="sigma">Sigma.</param>
        private static double RandomNormal(double mu, double sigma)
        {
            return InverseCumNormal(Rand.NextDouble(), mu, sigma);
        }

        /// <summary>
        /// Fast approximation for inverse cum normal
        /// </summary>
        /// <param name="p">probability</param>
        /// <param name="mu">Mean</param>
        /// <param name="sigma">std dev</param>
        private static double InverseCumNormal(double p, double mu, double sigma)
        {
            const double A1 = -3.969683028665376e+01;
            const double A2 = 2.209460984245205e+02;
            const double A3 = -2.759285104469687e+02;
            const double A4 = 1.383577518672690e+02;
            const double A5 = -3.066479806614716e+01;
            const double A6 = 2.506628277459239e+00;

            const double B1 = -5.447609879822406e+01;
            const double B2 = 1.615858368580409e+02;
            const double B3 = -1.556989798598866e+02;
            const double B4 = 6.680131188771972e+01;
            const double B5 = -1.328068155288572e+01;

            const double C1 = -7.784894002430293e-03;
            const double C2 = -3.223964580411365e-01;
            const double C3 = -2.400758277161838e+00;
            const double C4 = -2.549732539343734e+00;
            const double C5 = 4.374664141464968e+00;
            const double C6 = 2.938163982698783e+00;

            const double D1 = 7.784695709041462e-03;
            const double D2 = 3.224671290700398e-01;
            const double D3 = 2.445134137142996e+00;
            const double D4 = 3.754408661907416e+00;

            const double Xlow = 0.02425;
            const double Xhigh = 1.0 - Xlow;

            double z, r;

            if (p < Xlow)
            {
                // Rational approximation for the lower region 0<x<u_low
                z = Math.Sqrt(-2.0 * Math.Log(p));
                z = (((((C1 * z + C2) * z + C3) * z + C4) * z + C5) * z + C6) /
                    ((((D1 * z + D2) * z + D3) * z + D4) * z + 1.0);
            }
            else if (p <= Xhigh)
            {
                // Rational approximation for the central region u_low<=x<=u_high
                z = p - 0.5;
                r = z * z;
                z = (((((A1 * r + A2) * r + A3) * r + A4) * r + A5) * r + A6) * z /
                    (((((B1 * r + B2) * r + B3) * r + B4) * r + B5) * r + 1.0);
            }
            else
            {
                // Rational approximation for the upper region u_high<x<1
                z = Math.Sqrt(-2.0 * Math.Log(1.0 - p));
                z = -(((((C1 * z + C2) * z + C3) * z + C4) * z + C5) * z + C6) /
                    ((((D1 * z + D2) * z + D3) * z + D4) * z + 1.0);
            }

            // error (f_(z) - x) divided by the cumulative's derivative
            r = (CumN0(z) - p) * Math.Sqrt(2.0) * Math.Sqrt(Math.PI) * Math.Exp(0.5 * z * z);

            // Halley's method
            z -= r / (1 + (0.5 * z * r));

            return mu + (z * sigma);
        }

        /// <summary>
        /// Cumulative for a N(0,1) distribution
        /// </summary>
        /// <returns>The n0.</returns>
        /// <param name="x">The x coordinate.</param>
        private static double CumN0(double x)
        {
            const double B1 = 0.319381530;
            const double B2 = -0.356563782;
            const double B3 = 1.781477937;
            const double B4 = -1.821255978;
            const double B5 = 1.330274429;
            const double P = 0.2316419;
            const double C = 0.39894228;

            if (x >= 0.0)
            {
                double t = 1.0 / (1.0 + (P * x));
                return (1.0 - C * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * B5 + B4) + B3) + B2) + B1));
            }
            else
            {
                double t = 1.0 / (1.0 - P * x);
                return (C * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * B5 + B4) + B3) + B2) + B1));
            }
        }
    }
}