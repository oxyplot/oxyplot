// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandleStickAndVolumeSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("CandleStickAndVolumeSeries")]
    [Tags("Series")]
    public static class CandleStickAndVolumeSeriesExamples
    {
        [Example("Candles + Volume (combined volume), adjusting Y-axis")]
        public static Example CombinedVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (combined volume)", 
                VolumeStyle.Combined,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (combined volume), natural Y-axis")]
        public static Example CombinedVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (combined volume)", 
                VolumeStyle.Combined,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (stacked volume), adjusting Y-axis")]
        public static Example StackedVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (stacked volume)", 
                VolumeStyle.Stacked,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (stacked volume), natural Y-axis")]
        public static Example StackedVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (stacked volume)", 
                VolumeStyle.Stacked,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (+/- volume), adjusting Y-axis")]
        public static Example PosNegVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (+/- volume)",
                VolumeStyle.PositiveNegative,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (+/- volume), natural Y-axis")]
        public static Example PosNegVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (+/- volume)",
                VolumeStyle.PositiveNegative,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (volume not shown), adjusting Y-axis")]
        public static Example NoVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (volume not shown)", 
                VolumeStyle.None,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (volume not shown), natural Y-axis")]
        public static Example NoVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (volume not shown)", 
                VolumeStyle.None,
                naturalY: true,
                naturalV: true);
        }

        /// <summary>
        /// Creates the candle stick and volume series example.
        /// </summary>
        /// <returns>The candle stick and volume series example.</returns>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        /// <param name="n">N.</param>
        /// <param name="naturalY">If set to <c>true</c> natural y.</param>
        /// <param name="naturalV">If set to <c>true</c> natural v.</param>
        private static Example CreateCandleStickAndVolumeSeriesExample(
            string title, 
            VolumeStyle style, 
            int n = 10000,
            bool naturalY = false,
            bool naturalV = false)
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
            foreach (var bar in OhlcvItemGenerator.MRProcess(n))
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
                Key = series.BarAxisKey,
                StartPosition = 0.25,
                EndPosition = 1.0,
                Minimum = naturalY ? double.NaN : Ymin,
                Maximum = naturalY ? double.NaN : Ymax
            };
            var volAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.VolumeAxisKey,
                StartPosition = 0.0,
                EndPosition = 0.22,
                Minimum = naturalV ? double.NaN : 0,
                Maximum = naturalV ? double.NaN : 5000
            };

            switch (style)
            {
                case VolumeStyle.None:
                    barAxis.Key = null;
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
                    volAxis.Minimum = naturalV ? double.NaN : -5000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);

            if (naturalY == false)
            {
                timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);
            }

            var controller = new PlotController();
            controller.UnbindAll();
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

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }
}