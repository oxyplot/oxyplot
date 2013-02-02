// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
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
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;

namespace DateTimeDemo
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ExampleModel { get; set; }
        public PlotModel ExampleModel2 { get; set; }
        public Collection<DateValue> Data { get; set; }
        public Collection<TimeValue> Data2 { get; set; }
        public Collection<SunItem> SunData { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2015, 01, 01), 3600 * 24 * 14);
            ExampleModel2 = CreateModel2(new TimeSpan(0, 0, 0, 0), new TimeSpan(0, 24, 0, 0), 3600);

            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2011, 01, 01), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 05), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 02), 3600 );

            int year = DateTime.Now.Year;

            // Oslo
            SunData = CreateSunData(year, 59.91, 10.75, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

            // Honolulu
            // SunData = CreateSunData(year, 21.30694, -157.85833, TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"));
        }

        private Collection<SunItem> CreateSunData(int year, double lat, double lon, TimeZoneInfo tzi)
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

        private PlotModel CreateModel(DateTime start, DateTime end, double increment)
        {
            var tmp = new PlotModel("DateTime axis (PlotModel)");
            tmp.Axes.Add(new DateTimeAxis(AxisPosition.Bottom));
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left));

            // Create a random data collection
            var r = new Random();
            Data = new Collection<DateValue>();
            var date = start;
            while (date <= end)
            {
                Data.Add(new DateValue { Date = date, Value = r.NextDouble() });
                date = date.AddSeconds(increment);
            }

            // Create a line series
            var s1 = new LineSeries
                         {
                             StrokeThickness = 1,
                             MarkerSize = 3,
                             ItemsSource = Data,
                             DataFieldX = "Date",
                             DataFieldY = "Value",
                             MarkerStroke = OxyColors.ForestGreen,
                             MarkerType = MarkerType.Plus
                         };

            tmp.Series.Add(s1);
            return tmp;
        }

        private PlotModel CreateModel2(TimeSpan start, TimeSpan end, double increment)
        {
            var tmp = new PlotModel("TimeSpan axis (PlotModel)");
            tmp.Axes.Add(new TimeSpanAxis() { StringFormat = "h:mm" });

            // Create a random data collection
            var r = new Random();
            Data2 = new Collection<TimeValue>();
            var current = start;
            while (current <= end)
            {
                Data2.Add(new TimeValue { Time = current, Value = r.NextDouble() });
                current = current.Add(new TimeSpan(0, 0, (int)increment));
            }

            // Create a line series
            var s1 = new LineSeries
            {
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = Data2,
                DataFieldX = "Time",
                DataFieldY = "Value",
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus
            };

            tmp.Series.Add(s1);
            return tmp;
        }
    }

    public class DateValue
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

    public class TimeValue
    {
        public TimeSpan Time { get; set; }
        public double Value { get; set; }
    }

    public class SunItem
    {
        public DateTime Day { get; set; }
        public TimeSpan Sunrise { get; set; }
        public TimeSpan Sunset { get; set; }
    }
}