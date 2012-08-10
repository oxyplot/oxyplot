// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using OxyPlot;

namespace BarSeriesDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create some data
            Items = new Collection<Item>
                            {
                                new Item {Label = "Apples", Value1 = 37, Value2 = 12, Value3 = 19},
                                new Item {Label = "Pears", Value1 = 7, Value2 = 21, Value3 = 9},
                                new Item {Label = "Bananas", Value1 = 23, Value2 = 2, Value3 = 29}
                            };

            // Create the plot model
            var tmp = new PlotModel("Bar series") { LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            tmp.Axes.Add(new CategoryAxis { ItemsSource = Items, LabelField = "Label" });
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left) { MinimumPadding = 0, AbsoluteMinimum = 0 });

            // Add the series, note that the the BarSeries are using the same ItemsSource as the CategoryAxis.
            tmp.Series.Add(new ColumnSeries { Title = "2009", ItemsSource = Items, ValueField = "Value1" });
            tmp.Series.Add(new ColumnSeries { Title = "2010", ItemsSource = Items, ValueField = "Value2" });
            tmp.Series.Add(new ColumnSeries { Title = "2011", ItemsSource = Items, ValueField = "Value3" });

            Model1 = tmp;

            DataContext = this;
        }

        public Collection<Item> Items { get; set; }
        public PlotModel Model1 { get; set; }
    }

    public class Item
    {
        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
    }
}