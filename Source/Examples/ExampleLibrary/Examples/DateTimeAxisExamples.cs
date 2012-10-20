// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxisExamples.cs" company="OxyPlot">
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
using System;
using System.Collections.Generic;
using OxyPlot;

namespace ExampleLibrary
{
    using System.Collections.ObjectModel;
    using System.Globalization;

    [Examples("DateTimeAxis")]
    public static class DateTimeAxisExamples
    {
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }

        // [Example("DateTime Minimum bug")]
        public static PlotModel Example1()
        {
            var tmp = new PlotModel("Test");
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, TickStyle = TickStyle.Outside });
            DateTime dt = new DateTime(2010, 1, 1);
            tmp.Axes.Add(new DateTimeAxis(dt, dt.AddDays(1), AxisPosition.Bottom, null, null, DateTimeIntervalType.Hours)
            {
                MajorGridlineStyle = LineStyle.Solid,
                Angle = 90,
                StringFormat = "HH:mm",
                MajorStep = 1.0 / 24 / 2, // 1/24 = 1 hour, 1/24/2 = 30 minutes
                IsZoomEnabled = true,
                MaximumPadding = 0,
                MinimumPadding = 0,
                TickStyle = TickStyle.None
            });

            var ls = new LineSeries("Line1") { DataFieldX = "X", DataFieldY = "Y" };
            List<Item> ii = new List<Item>();

            for (int i = 0; i < 24; i++)
                ii.Add(new Item { X = dt.AddHours(i), Y = i * i });
            ls.ItemsSource = ii;
            tmp.Series.Add(ls);
            return tmp;
        }

        public class Item
        {
            public DateTime X { get; set; }
            public double Y { get; set; }
        }

        [Example("DateTime axis")]
        public static PlotModel DateTimeaxisPlotModel()
        {
            var start = new DateTime(2010, 01, 01);
            var end = new DateTime(2015, 01, 01);
            double increment = 3600 * 24 * 14;

            // Create a random data collection
            var r = new Random();
            var data = new Collection<DateValue>();
            var date = start;
            while (date <= end)
            {
                data.Add(new DateValue { Date = date, Value = r.NextDouble() });
                date = date.AddSeconds(increment);
            }

            var plotModel1 = new PlotModel("DateTime axis");
            var dateTimeAxis1 = new DateTimeAxis
                {
                    CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
                    FirstDayOfWeek = DayOfWeek.Monday,
                    Position = AxisPosition.Bottom
                };
            plotModel1.Axes.Add(dateTimeAxis1);
            var linearAxis1 = new LinearAxis();
            plotModel1.Axes.Add(linearAxis1);
            var lineSeries1 = new LineSeries
                {
                    Color = OxyColor.FromArgb(255, 78, 154, 6),
                    MarkerFill = OxyColor.FromArgb(255, 78, 154, 6),
                    MarkerStroke = OxyColors.ForestGreen,
                    MarkerType = MarkerType.Plus,
                    StrokeThickness = 1,
                    DataFieldX = "Date",
                    DataFieldY = "Value",
                    ItemsSource = data
                };
            plotModel1.Series.Add(lineSeries1);
            return plotModel1;
        }

        public class SunItem
        {
            public DateTime Day { get; set; }
            public TimeSpan Sunrise { get; set; }
            public TimeSpan Sunset { get; set; }
        }

        private static Collection<SunItem> CreateSunData(int year, double lat, double lon, TimeZoneInfo tzi = null)
        {
            var data = new Collection<SunItem>();
            var day = new DateTime(year, 1, 1);

            while (day.Year == year)
            {
                var sunrise = Sun.Calculate(day, lat, lon, true, tzi);
                var sunset = Sun.Calculate(day, lat, lon, false, tzi);
                data.Add(new SunItem { Day = day, Sunrise = sunrise - day, Sunset = sunset - day });
                day = day.AddDays(1);
            }
            return data;
        }

        [Example("Sunrise and sunset in Oslo")]
        public static PlotModel SunriseandsunsetinOslo()
        {
            int year = DateTime.Now.Year;
#if SILVERLIGHT || PCL
            var sunData = CreateSunData(year, 59.91, 10.75);
#else
            var sunData = CreateSunData(year, 59.91, 10.75, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));
#endif
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Sunrise and sunset in Oslo";

#if SILVERLIGHT
            plotModel1.Subtitle = "UTC time";
#endif

            var dateTimeAxis1 = new DateTimeAxis
                {
                    CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
                    FirstDayOfWeek = DayOfWeek.Monday,
                    IntervalType = DateTimeIntervalType.Months,
                    MajorGridlineStyle = LineStyle.Solid,
                    Position = AxisPosition.Bottom,
                    StringFormat = "MMM"
                };
            plotModel1.Axes.Add(dateTimeAxis1);
            var timeSpanAxis1 = new TimeSpanAxis { MajorGridlineStyle = LineStyle.Solid, Maximum = 86400, Minimum = 0, StringFormat = "h:mm" };
            plotModel1.Axes.Add(timeSpanAxis1);
            var areaSeries1 = new AreaSeries
                {
                    ItemsSource = sunData,
                    DataFieldX = "Day",
                    DataFieldY = "Sunrise",
                    DataFieldX2 = "Day",
                    DataFieldY2 = "Sunset",
                    Fill = OxyColor.FromArgb(128, 255, 255, 0),
                    Color = OxyColors.Black
                };
            plotModel1.Series.Add(areaSeries1);
            return plotModel1;
        }

    }
}