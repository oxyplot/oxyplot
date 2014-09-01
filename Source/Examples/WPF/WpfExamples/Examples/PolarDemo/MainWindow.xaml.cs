// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PolarDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Creates a polar plot.")]
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.MaxAngle = Math.PI * 2;
            this.MajorStep = Math.PI / 4;
            this.MinorStep = Math.PI / 16;
            this.PI = Math.PI;
            this.MyModel = this.CreateModel();
            this.SpiralPoints = ((FunctionSeries)this.MyModel.Series[0]).Points;
        }

        /// <summary>
        /// Gets or sets MajorStep.
        /// </summary>
        public double MajorStep { get; set; }

        /// <summary>
        /// Gets or sets MaxAngle.
        /// </summary>
        public double MaxAngle { get; set; }

        /// <summary>
        /// Gets or sets MinorStep.
        /// </summary>
        public double MinorStep { get; set; }

        /// <summary>
        /// Gets or sets MyModel.
        /// </summary>
        public PlotModel MyModel { get; set; }

        /// <summary>
        /// Gets or sets PI.
        /// </summary>
        public double PI { get; set; }

        /// <summary>
        /// Gets or sets SpiralPoints.
        /// </summary>
        public IList<DataPoint> SpiralPoints { get; set; }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns>A PlotModel.</returns>
        private PlotModel CreateModel()
        {
            var model = new PlotModel
            {
                Title = "Polar plot",
                Subtitle = "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π",
                PlotType = PlotType.Polar,
                PlotMargins = new OxyThickness(20, 20, 4, 40),
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            model.Axes.Add(
                new AngleAxis
                {
                    Minimum = 0,
                    Maximum = this.MaxAngle,
                    MajorStep = this.MajorStep,
                    MinorStep = this.MinorStep,
                    FormatAsFractions = true,
                    FractionUnit = Math.PI,
                    FractionUnitSymbol = "π"
                });
            model.Axes.Add(new MagnitudeAxis());
            model.Series.Add(new FunctionSeries(t => t, t => t, 0, Math.PI * 6, 0.01));
            return model;
        }

    }
}