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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2015, 01, 01), 3600 * 24 * 14);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2011, 01, 01), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 05), 3600 * 24);
            //ExampleModel = CreateModel(new DateTime(2010, 01, 01), new DateTime(2010, 01, 02), 3600 );
        }

        private PlotModel CreateModel(DateTime start, DateTime end, double increment)
        {
            var tmp = new PlotModel("DateTimeDemo");
            tmp.Axes.Add(new DateTimeAxis(AxisPosition.Bottom));

            // Create a random data collection
            var r = new Random();
            var data = new Collection<DateValue>();
            var date = start;
            while (date <= end)
            {
                data.Add(new DateValue { Date = date, Value = r.NextDouble() });
                date = date.AddSeconds(increment);
            }

            // Create a line series
            var s1 = new LineSeries
                         {
                             StrokeThickness = 1,
                             MarkerSize = 3,
                             ItemsSource = data,
                             DataFieldX = "Date",
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
}
