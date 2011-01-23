using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ExampleModel { get; set; }
        public PlotModel ExampleModel2 { get; set; }
        public Collection<DateValue> Data { get; set; }
        public Collection<TimeValue> Data2 { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2015, 01, 01), 3600 * 24 * 14);
            ExampleModel2 = CreateModel2(new TimeSpan(0, 0, 0, 0), new TimeSpan(0, 24, 0, 0), 3600);

            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2011, 01, 01), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 05), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 02), 3600 );
        }

        private PlotModel CreateModel(DateTime start, DateTime end, double increment)
        {
            var tmp = new PlotModel("DateTime axis (PlotModel)");
            tmp.Axes.Add(new DateTimeAxis());

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
}
