// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeAxisExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("DateTimeAxis"), Tags("Axes")]
    public static class DateTimeAxisExamples
    {
        public class DateValue
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }

        [Example("Default StringFormat")]
        public static PlotModel DefaultValues()
        {
            return CreateExample(7, null);
        }

        [Example("StringFormat 'MMM dd\\nyyyy'")]
        public static PlotModel StringFormat()
        {
            return CreateExample(7, "MMM dd\nyyyy");
        }

        private static PlotModel CreateExample(int days, string stringFormat)
        {
            var m = new PlotModel();
            var startTime = new DateTime(2000, 1, 1);
            var min = DateTimeAxis.ToDouble(startTime);
            var max = min + days;
            m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum = min, Maximum = max, StringFormat = stringFormat });
            m.Axes.Add(new DateTimeAxis { Position = AxisPosition.Left, Minimum = min, Maximum = max, StringFormat = stringFormat });
            return m;
        }

        // [Example("DateTime Minimum bug")]
        public static PlotModel Example1()
        {
            var tmp = new PlotModel { Title = "Test" };
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, TickStyle = TickStyle.Outside });
            var dt = new DateTime(2010, 1, 1);
            tmp.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(dt),
                Maximum = DateTimeAxis.ToDouble(dt.AddDays(1)),
                IntervalType = DateTimeIntervalType.Hours,
                MajorGridlineStyle = LineStyle.Solid,
                Angle = 90,
                StringFormat = "HH:mm",
                MajorStep = 1.0 / 24 / 2, // 1/24 = 1 hour, 1/24/2 = 30 minutes
                IsZoomEnabled = true,
                MaximumPadding = 0,
                MinimumPadding = 0,
                TickStyle = TickStyle.None
            });

            var ls = new LineSeries { Title = "Line1", DataFieldX = "X", DataFieldY = "Y" };
            var ii = new List<Item>();

            for (int i = 0; i < 24; i++)
                ii.Add(new Item { X = dt.AddHours(i), Y = i * i });
            ls.ItemsSource = ii;
            tmp.Series.Add(ls);
            return tmp;
        }

        [Example("TimeZone adjustments")]
        public static PlotModel DaylightSavingsBreak()
        {
            var m = new PlotModel();

            var xa = new DateTimeAxis { Position = AxisPosition.Bottom };
            // TimeZone not available in PCL...

            m.Axes.Add(xa);
            m.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var ls = new LineSeries { MarkerType = MarkerType.Circle };
            m.Series.Add(ls);

            // set the origin of the curve to 2013-03-31 00:00:00 (UTC)
            var o = new DateTime(2013, 3, 31, 0, 0, 0, DateTimeKind.Utc);

            // add points at 10min intervals
            // at 2am the clocks are turned forward 1 hour (W. Europe Standard Time)
            for (int i = 0; i < 400; i += 10)
            {
                var time = o.AddMinutes(i);
                ls.Points.Add(DateTimeAxis.CreateDataPoint(time, i));
            }

            return m;
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
            var r = new Random(13);
            var data = new Collection<DateValue>();
            var date = start;
            while (date <= end)
            {
                data.Add(new DateValue { Date = date, Value = r.NextDouble() });
                date = date.AddSeconds(increment);
            }

            var plotModel1 = new PlotModel { Title = "DateTime axis" };
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

        private static Collection<SunItem> CreateSunData(int year, double lat, double lon, Func<DateTime, DateTime> utcToLocalTime)
        {
            var data = new Collection<SunItem>();
            var day = new DateTime(year, 1, 1);

            while (day.Year == year)
            {
                var sunrise = Sun.Calculate(day, lat, lon, true, utcToLocalTime);
                var sunset = Sun.Calculate(day, lat, lon, false, utcToLocalTime);
                data.Add(new SunItem { Day = day, Sunrise = sunrise - day, Sunset = sunset - day });
                day = day.AddDays(1);
            }

            return data;
        }

        public static bool IsDaylightSaving(DateTime time)
        {
            // Daylight saving starts last sunday in March and ends last sunday in October
            // http://en.wikipedia.org/wiki/Daylight_saving_time
            var start = new DateTime(time.Year, 3, 31, 2, 0, 0);
            start = start.AddDays(-(int)start.DayOfWeek);
            var end = new DateTime(time.Year, 10, 31, 3, 0, 0);
            end = end.AddDays(-(int)end.DayOfWeek);
            return time >= start && time <= end;
        }

        [Example("Sunrise and sunset in Oslo")]
        public static PlotModel SunriseandsunsetinOslo()
        {
            int year = DateTime.Now.Year;

            // Convert UTC time to Western European Time (WET)
            Func<DateTime, DateTime> utcToLocalTime = utc => utc.AddHours(IsDaylightSaving(utc) ? 2 : 1);

            var sunData = CreateSunData(year, 59.91, 10.75, utcToLocalTime);

            var plotModel1 = new PlotModel { Title = "Sunrise and sunset in Oslo", Subtitle = "UTC time" };

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

        [Example("LabelFormatter")]
        public static PlotModel LabelFormatter()
        {
            var model = new PlotModel { Title = "Using LabelFormatter to format labels by day of week" };
            model.Axes.Add(new DateTimeAxis { LabelFormatter = x => DateTimeAxis.ToDateTime(x).DayOfWeek.ToString().Substring(0, 3) });
            var series = new LineSeries();
            model.Series.Add(series);
            for (int i = 0; i < 7; i++)
            {
                var time = new DateTime(2014, 9, 10).AddDays(i);
                double x = DateTimeAxis.ToDouble(time);
                double y = Math.Sin(i * i);
                series.Points.Add(new DataPoint(x, y));
            }

            return model;
        }
    }
}