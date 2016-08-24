// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.ObjectModel;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("TimeSpanAxis"), Tags("Axes")]
    public static class TimeSpanAxisExamples
    {
        public class TimeValue
        {
            public TimeSpan Time { get; set; }
            public double Value { get; set; }
        }

        [Example("Default StringFormat")]
        public static PlotModel TimeSpanaxisPlotModelDefault()
        {
            return TimeSpanaxisPlotModel(null);
        }

        [Example("StringFormat = 'h:mm'")]
        public static PlotModel TimeSpanaxisPlotModel1()
        {
            return TimeSpanaxisPlotModel("h:mm");
        }

        private static PlotModel TimeSpanaxisPlotModel(string stringFormat)
        {
            var start = new TimeSpan(0, 0, 0, 0);
            var end = new TimeSpan(0, 24, 0, 0);
            double increment = 3600;

            // Create a random data collection
            var r = new Random(7);
            var data = new Collection<TimeValue>();
            var current = start;
            while (current <= end)
            {
                data.Add(new TimeValue { Time = current, Value = r.NextDouble() });
                current = current.Add(new TimeSpan(0, 0, (int)increment));
            }

            var plotModel1 = new PlotModel { Title = "TimeSpan axis" };
            var timeSpanAxis1 = new TimeSpanAxis { Position = AxisPosition.Bottom, StringFormat = stringFormat };
            plotModel1.Axes.Add(timeSpanAxis1);
            var linearAxis1 = new LinearAxis { Position = AxisPosition.Left };
            plotModel1.Axes.Add(linearAxis1);
            var lineSeries1 = new LineSeries
            {
                Color = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerFill = OxyColor.FromArgb(255, 78, 154, 6),
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus,
                StrokeThickness = 1,
                DataFieldX = "Time",
                DataFieldY = "Value",
                ItemsSource = data
            };
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

    }
}