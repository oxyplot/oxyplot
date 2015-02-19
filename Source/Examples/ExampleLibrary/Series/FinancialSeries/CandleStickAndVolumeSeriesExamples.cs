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
        [Example("Candles + Volume (combined volume)")]
        public static Example CombinedVolume()
        {
            return CreateCandleStickAndVolumeSeriesExample("Candles + Volume (combined volume)", VolumeStyle.Combined);
        }

        [Example("Candles + Volume (stacked volume)")]
        public static Example StackedVolume()
        {
            return CreateCandleStickAndVolumeSeriesExample("Candles + Volume (stacked volume)", VolumeStyle.Stacked);
        }

        [Example("Candles + Volume (+/- volume)")]
        public static Example PosNegVolume()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (+/- volume)",
                VolumeStyle.PositiveNegative);
        }

        [Example("Candles + Volume (volume not shown)")]
        public static Example NoVolume()
        {
            return CreateCandleStickAndVolumeSeriesExample("Candles + Volume (volume not shown)", VolumeStyle.None);
        }

        /// <summary>
        /// Creates the candlestick and volume series example.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="style">The style.</param>
        /// <param name="n">The number of bars.</param>
        /// <returns>
        /// An example.
        /// </returns>
        private static Example CreateCandleStickAndVolumeSeriesExample(string title, VolumeStyle style, int n = 10000)
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

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }
}