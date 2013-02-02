// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
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
        public IList<IDataPoint> SpiralPoints { get; set; }

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// A PlotModel.
        /// </returns>
        private PlotModel CreateModel()
        {
            var model = new PlotModel("Polar plot", "Archimedean spiral with equation r(θ) = θ for 0 < θ < 6π")
                {
                    PlotType = PlotType.Polar,
                    PlotMargins = new OxyThickness(20, 20, 4, 40),
                    PlotAreaBorderThickness = 0
                };
            model.Axes.Add(
                new AngleAxis(0, this.MaxAngle, this.MajorStep, this.MinorStep)
                    {
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