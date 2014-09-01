// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DateTimeDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Plotting with DateTime axes.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.ExampleModel = this.CreateModel(new DateTime(2010, 01, 01), new DateTime(2015, 01, 01), 3600 * 24 * 14);
            this.ExampleModel2 = this.CreateModel2(new TimeSpan(0, 0, 0, 0), new TimeSpan(0, 24, 0, 0), 3600);

            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2011, 01, 01), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 05), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 02), 3600 );

            int year = DateTime.Now.Year;

            // Oslo
            this.SunData = this.CreateSunData(year, 59.91, 10.75, TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

            // Honolulu
            // SunData = CreateSunData(year, 21.30694, -157.85833, TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"));
        }

        public PlotModel ExampleModel { get; set; }
        
        public PlotModel ExampleModel2 { get; set; }
        
        public Collection<DateValue> Data { get; set; }
        
        public Collection<TimeValue> Data2 { get; set; }

        public Collection<SunItem> SunData { get; set; }

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
            var tmp = new PlotModel { Title = "DateTime axis (PlotModel)" };
            tmp.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // Create a random data collection
            var r = new Random(13);
            this.Data = new Collection<DateValue>();
            var date = start;
            while (date <= end)
            {
                this.Data.Add(new DateValue { Date = date, Value = r.NextDouble() });
                date = date.AddSeconds(increment);
            }

            // Create a line series
            var s1 = new LineSeries
                         {
                             StrokeThickness = 1,
                             MarkerSize = 3,
                             ItemsSource = this.Data,
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
            var tmp = new PlotModel { Title = "TimeSpan axis (PlotModel)" };
            tmp.Axes.Add(new TimeSpanAxis() { StringFormat = "h:mm" });

            // Create a random data collection
            var r = new Random(13);
            this.Data2 = new Collection<TimeValue>();
            var current = start;
            while (current <= end)
            {
                this.Data2.Add(new TimeValue { Time = current, Value = r.NextDouble() });
                current = current.Add(new TimeSpan(0, 0, (int)increment));
            }

            // Create a line series
            var s1 = new LineSeries
            {
                StrokeThickness = 1,
                MarkerSize = 3,
                ItemsSource = this.Data2,
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