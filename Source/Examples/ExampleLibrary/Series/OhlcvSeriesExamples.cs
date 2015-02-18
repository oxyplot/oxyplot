// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OHLCSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;


    [Examples ("OHLCV Related Series"), Tags ("Series")]
    public static class OhlcvSeriesExamples
    {
        [Example ("Candles + Volume (combined volume)")]
        public static Example CombinedVolume()
        {
            return CreateBarVolSeries ("Candles + Volume (combined volume)", VolumeStyle.Combined);
        }


        [Example ("Candles + Volume (stacked volume)")]
        public static Example StackedVolume()
        {
            return CreateBarVolSeries ("Candles + Volume (stacked volume)", VolumeStyle.Stacked);
        }


        [Example ("Candles + Volume (+/- volume)")]
        public static Example PosNegVolume()
        {
            return CreateBarVolSeries ("Candles + Volume (+/- volume)", VolumeStyle.PositiveNegative);
        }


        [Example ("Candles + Volume (volume not shown)")]
        public static Example NoVolume()
        {
            return CreateBarVolSeries ("Candles + Volume (volume not shown)", VolumeStyle.None);
        }


        [Example ("Just Volume (combined)")]
        public static Example JustVolumeCombined()
        {
            return CreateVolumeSeries ("Just Volume (combined)", VolumeStyle.Combined);
        }


        [Example ("Just Volume (stacked)")]
        public static Example JustVolumeStacked()
        {
            return CreateVolumeSeries ("Just Volume (stacked)", VolumeStyle.Stacked);
        }


        [Example ("Just Volume (+/-)")]
        public static Example JustVolumePosNeg()
        {
            return CreateVolumeSeries ("Just Volume (+/-)", VolumeStyle.PositiveNegative);
        }


        /// <summary>
        /// Creates the bar & vol series.
        /// </summary>
        /// <returns>The bar vol series.</returns>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        private static Example CreateBarVolSeries(string title, VolumeStyle style)
        {
            var pm = new PlotModel { Title = title };

            var series = new CandleStickAndVolumeSeries 
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                SeparatorColor = OxyColors.Gray,
                SeparatorLineStyle = LineStyle.Dash,
                VolumeStyle = style
            };

            // create bars
            var n = 10000;
            foreach (var bar in OHLCVBarGenerator.MRProcess(n))
            {
                series.Append(bar);
            }


            // create visible window
            var Istart = n - 200;
            var Iend = n - 120;
            var Ymin = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.Low).Min();
            var Ymax = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.High).Max();
            var Xmin = series.Items[Istart].X;
            var Xmax = series.Items[Iend].X;

            // setup axes
            var timeAxis = new DateTimeAxis 
            { 
                Position = AxisPosition.Bottom,
                Minimum = Xmin,
                Maximum = Xmax
            };
            var barAxis = new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Key = "Bars", 
                StartPosition = 0.25, 
                EndPosition = 1.0,
                Minimum = Ymin, 
                Maximum = Ymax
            };
            var volAxis = new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Key = "Volume", 
                StartPosition = 0.0, 
                EndPosition = 0.22,
                Minimum = 0, 
                Maximum = 5000
            };

            switch (style)
            {
                case VolumeStyle.None:
                    barAxis.StartPosition = 0.0;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    break;

                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = -5000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);
            timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);

            var controller = new PlotController();
            controller.InputCommandBindings.Clear ();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }


        /// <summary>
        /// Creates a volume series.
        /// </summary>
        /// <returns>The volume series.</returns>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        private static Example CreateVolumeSeries(string title, VolumeStyle style)
        {
            var pm = new PlotModel { Title = title };

            var series = new VolumeSeries 
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                VolumeStyle = style
            };

            // create bars
            var n = 10000;
            foreach (var bar in OHLCVBarGenerator.MRProcess(n))
            {
                series.Append(bar);
            }

            // create visible window
            var Istart = n - 200;
            var Iend = n - 120;
            var Xmin = series.Items[Istart].X;
            var Xmax = series.Items[Iend].X;

            // setup axes
            var timeAxis = new DateTimeAxis 
            { 
                Position = AxisPosition.Bottom,
                Minimum = Xmin,
                Maximum = Xmax
            };
            var volAxis = new LinearAxis 
            { 
                Position = AxisPosition.Left, 
                Key = "Volume", 
                StartPosition = 0.0, 
                EndPosition = 1.0,
                Minimum = 0, 
                Maximum = 10000
            };

            switch (style)
            {
                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = -10000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);

            var controller = new PlotController();
            controller.InputCommandBindings.Clear();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }


        /// <summary>
        /// Adjusts the Y extent.
        /// </summary>
        /// <param name="series">Series.</param>
        /// <param name="xaxis">Xaxis.</param>
        /// <param name="yaxis">Yaxis.</param>
        private static void AdjustYExtent(CandleStickAndVolumeSeries series, DateTimeAxis xaxis, LinearAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = (ymax - ymin);
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }

    }


    /// <summary>
    /// Creates realistic bars
    /// </summary>
    public static class OHLCVBarGenerator
    {
        /// <summary>
        /// Create bars governed by a MR process
        /// </summary>
        /// <returns>The process.</returns>
        /// <param name="n">N.</param>
        /// <param name="x0">X0.</param>
        /// <param name="v0">V0.</param>
        /// <param name="csigma">Csigma.</param>
        /// <param name="esigma">Esigma.</param>
        /// <param name="kappa">Kappa.</param>
        public static IEnumerable<OhlcvItem> MRProcess(
            int n,
            double x0 = 100.0,
            double v0 = 500,
            double csigma = 0.50, 
            double esigma = 0.75, 
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

                var dp = (close - open);
                var v = v0 * Math.Exp(Math.Abs(dp) / csigma);
                var dir = (dp < 0) ?
					-Math.Min(-dp / esigma, 1.0) :
					Math.Min(dp / esigma, 1.0);

                var skew = (dir + 1) / 2.0;
                var buyvol = skew * v;
                var sellvol = (1 - skew) * v;

                var nowT = baseT.AddSeconds(ti);
                var t = DateTimeAxis.ToDouble(nowT);
                yield return new OhlcvItem(t, open, high, low, close, buyvol, sellvol);
            }
        }




        /// <summary>
        /// Minimum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
        private static double Min(double a, double b, double c, double d)
        {
            return Math.Min(a, Math.Min(b, Math.Min(c, d)));
        }


        /// <summary>
        /// Maximum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
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
            return InverseCumNormal(_rand.NextDouble(), mu, sigma);
        }


        /// <summary>
        /// fast approximation for inverse cum normal
        /// </summary>
        /// <param name="u">probability</param>
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
                z = Math.Sqrt(-2.0 * Math.Log (p));
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


        static Random _rand = new Random();
    }

}