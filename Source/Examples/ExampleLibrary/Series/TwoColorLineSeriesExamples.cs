// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides examples for the <see cref="TwoColorLineSeries" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using ExampleLibrary.Utilities;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.Legends;

    /// <summary>
    /// Provides examples for the <see cref="TwoColorLineSeries" />.
    /// </summary>
    [Examples("TwoColorLineSeries"), Tags("Series")]
    public class TwoColorLineSeriesExamples
    {
        /// <summary>
        /// Creates an example showing temperatures by a red/blue line.
        /// </summary>
        /// <returns>A <see cref="PlotModel" />.</returns>
        [Example("Temperatures")]
        [DocumentationExample("Series/TwoColorLineSeries")]
        public static PlotModel TwoColorLineSeries()
        {
            var model = new PlotModel { Title = "TwoColorLineSeries" };
            var l = new Legend
            {
                LegendSymbolLength = 24
            };

            model.Legends.Add(l);

            var s1 = new TwoColorLineSeries
            {
                Title = "Temperature at Eidesmoen, December 1986.",
                TrackerFormatString = "December {2:0}: {4:0.0} °C",
                Color = OxyColors.Red,
                Color2 = OxyColors.LightBlue,
                StrokeThickness = 3,
                Limit = 0,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Black,
                MarkerStrokeThickness = 1.5,
            };

            var temperatures = new[] { 5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0, -5 };

            for (int i = 0; i < temperatures.Length; i++)
            {
                s1.Points.Add(new DataPoint(i + 1, temperatures[i]));
            }

            model.Series.Add(s1);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature", Unit = "°C", ExtraGridlines = new[] { 0.0 } });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Date" });

            return model;
        }

        [Example("Temperatures (Y Axis reversed)")]
        public static PlotModel TwoColorLineSeriesReversed()
        {
            return TwoColorLineSeries().ReverseYAxis();
        }
    }
}
