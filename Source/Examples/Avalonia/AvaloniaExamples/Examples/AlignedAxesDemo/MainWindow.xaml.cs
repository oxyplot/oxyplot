// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.AlignedAxesDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using Avalonia;
    using AvaloniaExamples;
    using Avalonia.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Aligning plot margins from desired axis widths.")]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            var models = new { Model0 = CreatePlotModel(0, 10), Model1 = CreatePlotModel(0, 1e8) };
            
            // TODO: align the vertical axis size without setting PlotMargins
            models.Model0.PlotMargins = models.Model1.PlotMargins = new OxyThickness(70, 40, 20, 20);

            this.DataContext = models;
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

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