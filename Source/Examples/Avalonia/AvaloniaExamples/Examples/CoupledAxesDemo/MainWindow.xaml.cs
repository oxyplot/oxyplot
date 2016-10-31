// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AvaloniaExamples.Examples.CoupledAxesDemo
{
    using System;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using AvaloniaExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows how to keep two axes in sync.")]
    public partial class MainWindow : Avalonia.Controls.Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.Model1 = new PlotModel { Title = "Model 1" };
            this.Model2 = new PlotModel { Title = "Model 2" };
            var axis1 = new LinearAxis { Position = AxisPosition.Bottom };
            var axis2 = new LinearAxis { Position = AxisPosition.Bottom };
            this.Model1.Axes.Add(axis1);
            this.Model2.Axes.Add(axis2);
            this.Model1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 1000));
            this.Model2.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, 0, 10, 1000));

            bool isInternalChange = false;
            axis1.AxisChanged += (s, e) =>
                {
                    if (isInternalChange)
                    {
                        return;
                    }

                    isInternalChange = true;
                    axis2.Zoom(axis1.ActualMinimum, axis1.ActualMaximum);
                    this.Model2.InvalidatePlot(false);
                    isInternalChange = false;
                };

            axis2.AxisChanged += (s, e) =>
            {
                if (isInternalChange)
                {
                    return;
                }

                isInternalChange = true;
                axis1.Zoom(axis2.ActualMinimum, axis2.ActualMaximum);
                this.Model1.InvalidatePlot(false);
                isInternalChange = false;
            };

            this.DataContext = new { Model1, Model2 };
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        public PlotModel Model1 { get; set; }

        public PlotModel Model2 { get; set; }
    }
}