// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("VolumeSeries")]
    [Tags("Series")]
    public static class VolumeSeriesExamples
    {
        [Example("Just Volume (combined)")]
        public static Example JustVolumeCombined()
        {
            return CreateVolumeSeries("Just Volume (combined)", VolumeStyle.Combined);
        }

        [Example("Just Volume (stacked)")]
        public static Example JustVolumeStacked()
        {
            return CreateVolumeSeries("Just Volume (stacked)", VolumeStyle.Stacked);
        }

        [Example("Just Volume (+/-)")]
        public static Example JustVolumePositiveNegative()
        {
            return CreateVolumeSeries("Just Volume (+/-)", VolumeStyle.PositiveNegative);
        }

        /// <summary>
        /// Creates a volume series example.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        /// <param name="n">The number of bars.</param>
        /// <returns>
        /// A volume series.
        /// </returns>
        private static Example CreateVolumeSeries(string title, VolumeStyle style, int n = 10000)
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
            foreach (var bar in OhlcvItemGenerator.MRProcess(n))
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
    }
}