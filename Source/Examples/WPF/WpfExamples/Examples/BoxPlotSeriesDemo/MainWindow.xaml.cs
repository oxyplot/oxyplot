// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Windows.Media;
using OxyPlot.Wpf;

namespace BoxPlotSeriesDemo
{
    using System.Collections.ObjectModel;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows boxplot series.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Create some data
            this.Items = new Collection<BoxPlotItem>
                            {
                                new BoxPlotItem(1, 13.0, 15.5, 17.0, 18.5, 19.5) { Mean = 18.0 },
                                new BoxPlotItem(2, 13.0, 15.5, 17.0, 18.5, 19.5),
                                new BoxPlotItem(3, 12.0, 13.5, 15.5, 18.0, 20.0) { Mean = 14.5 },
                                new BoxPlotItem(4, 12.0, 13.5, 15.5, 18.0, 20.0) { Mean = 14.5, Outliers = new List<double> { 11.0, 21.0, 21.5 } },
                                new BoxPlotItem(5, 13.5, 14.0, 14.5, 15.5, 16.5) { Outliers = new List<double> { 17.5, 18.0, 19.0 } }
                            };

            // Create the plot model
            var tmp = new PlotModel { Title = "BoxPlot series", LegendPlacement = LegendPlacement.Outside, LegendPosition = LegendPosition.RightTop, LegendOrientation = LegendOrientation.Vertical };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.3, MaximumPadding = 0.3, AbsoluteMinimum = 0 });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, MinimumPadding = 0.3, MaximumPadding = 0.3, AbsoluteMinimum = 0 });

            // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
            tmp.Series.Add(new BoxPlotSeries { Title = "Values", ItemsSource = this.Items, Fill = Colors.LightBlue.ToOxyColor() });

            this.Model1 = tmp;

            this.DataContext = this;
        }

        public Collection<BoxPlotItem> Items { get; set; }

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