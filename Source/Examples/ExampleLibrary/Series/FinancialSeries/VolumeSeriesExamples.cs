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
        [Example("Just Volume (combined), fixed axis")]
        public static Example JustVolumeCombined_Fixed()
        {
            return CreateVolumeSeries("Just Volume (combined)", VolumeStyle.Combined, natural: false);
        }

        [Example("Just Volume (combined), natural axis")]
        public static Example JustVolumeCombined_Natural()
        {
            return CreateVolumeSeries("Just Volume (combined)", VolumeStyle.Combined, natural: true);
        }

        [Example("Just Volume (stacked), fixed axis")]
        public static Example JustVolumeStacked_Fixed()
        {
            return CreateVolumeSeries("Just Volume (stacked)", VolumeStyle.Stacked, natural: false);
        }

        [Example("Just Volume (stacked), natural axis")]
        public static Example JustVolumeStacked_Natural()
        {
            return CreateVolumeSeries("Just Volume (stacked)", VolumeStyle.Stacked, natural: true);
        }

        [Example("Just Volume (+/-), fixed axis")]
        public static Example JustVolumePositiveNegative_Fixed()
        {
            return CreateVolumeSeries("Just Volume (+/-)", VolumeStyle.PositiveNegative, natural: false);
        }

        [Example("Just Volume (+/-), natural axis")]
        public static Example JustVolumePositiveNegative_Natural()
        {
            return CreateVolumeSeries("Just Volume (+/-)", VolumeStyle.PositiveNegative, natural: true);
        }

        /// <summary>
        /// Creates the volume series.
        /// </summary>
        /// <returns>The volume series.</returns>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        /// <param name="n">N.</param>
        /// <param name="natural">If set to <c>true</c> natural.</param>
        /// <param name="transposed">If set to <c>true</c> transposed.</param>
        private static Example CreateVolumeSeries(
            string title, 
            VolumeStyle style, 
            int n = 10000,
            bool natural = false)
        {
            var pm = new PlotModel { Title = title };

            var series = new VolumeSeries
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                VolumeStyle = style,
                Title = "VolumeSeries",
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
                StartPosition = 0.0,
                EndPosition = 1.0,
                Minimum = natural ? double.NaN : 0,
                Maximum = natural ? double.NaN : 10000
            };

            switch (style)
            {
                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = natural ? double.NaN : -10000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }
    }
}
