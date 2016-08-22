// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ScrollViewerDemo
{
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows a plot inside a ScrollViewer.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            var plotModel = new PlotModel { Title = "Plot in ScrollViewer" };
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });

            this.Plot = plotModel;
            this.DataContext = this;
        }

        public PlotModel Plot { get; set; }
    }
}