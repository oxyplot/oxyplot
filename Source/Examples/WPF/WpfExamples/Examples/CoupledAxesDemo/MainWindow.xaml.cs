// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

namespace CoupledAxesDemo
{
    using System;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows how to keep two axes in sync.")]
    public partial class MainWindow : Window
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
        }

        public PlotModel Model1 { get; set; }

        public PlotModel Model2 { get; set; }
    }
}