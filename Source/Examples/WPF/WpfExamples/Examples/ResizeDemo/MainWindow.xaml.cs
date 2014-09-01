// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ResizeDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Demonstrates resizing of the PlotView with grid splitters.")]
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.Model1 = CreateModel("Model 1");
            this.Model2 = CreateModel("Model 2");
            this.Model3 = CreateModel("Model 3");
        }

        /// <summary>
        /// Gets the left model.
        /// </summary>
        public PlotModel Model1 { get; private set; }

        /// <summary>
        /// Gets the right-top model.
        /// </summary>
        public PlotModel Model2 { get; private set; }

        /// <summary>
        /// Gets the right-bottom model.
        /// </summary>
        public PlotModel Model3 { get; private set; }

        /// <summary>
        /// Creates a sample model.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>A <see cref="PlotModel" />.</returns>
        private static PlotModel CreateModel(string title)
        {
            var model = new PlotModel { Title = title };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 1000));
            model.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, 0, 10, 1000));
            return model;
        }
    }
}