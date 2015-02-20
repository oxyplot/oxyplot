// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AlignedAxesDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Aligning plot margins from desired axis widths.")]
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Model0 = CreatePlotModel(0, 10);
            this.Model1 = CreatePlotModel(0, 1e8);

            // TODO: align the vertical axis size without setting PlotMargins
            this.Model0.PlotMargins = this.Model1.PlotMargins = new OxyThickness(70, 40, 20, 20);

            this.DataContext = this;
        }

        public PlotModel Model0 { get; private set; }

        public PlotModel Model1 { get; private set; }

        private static PlotModel CreatePlotModel(double min, double max)
        {
            var model = new PlotModel();
            var verticalAxis = new LinearAxis { Position = AxisPosition.Left, Minimum = min, Maximum = max };
            model.Axes.Add(verticalAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new FunctionSeries(x => Math.Sin(x * Math.PI * 4) * Math.Sin(x * Math.PI * 4) * Math.Sqrt(x) * max, 0, 1, 1000));
            return model;
        }
    }
}