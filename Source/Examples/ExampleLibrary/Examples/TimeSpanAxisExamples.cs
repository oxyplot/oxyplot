// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanAxisExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace ExampleLibrary
{
    using System;
    using System.Collections.ObjectModel;

    using OxyPlot;

    [Examples("TimeSpanAxis")]
    public static class TimeSpanAxisExamples
    {
        public class TimeValue
        {
            public TimeSpan Time { get; set; }
            public double Value { get; set; }
        }

        [Example("TimeSpan axis")]
        public static PlotModel TimeSpanaxisPlotModel()
        {
            var start = new TimeSpan(0, 0, 0, 0);
            var end = new TimeSpan(0, 24, 0, 0);
            double increment = 3600;

            // Create a random data collection
            var r = new Random();
            var data = new Collection<TimeValue>();
            var current = start;
            while (current <= end)
            {
                data.Add(new TimeValue { Time = current, Value = r.NextDouble() });
                current = current.Add(new TimeSpan(0, 0, (int)increment));
            }

            var plotModel1 = new PlotModel("TimeSpan axis");
            var timeSpanAxis1 = new TimeSpanAxis { Position = AxisPosition.Bottom, StringFormat = "h:mm" };
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