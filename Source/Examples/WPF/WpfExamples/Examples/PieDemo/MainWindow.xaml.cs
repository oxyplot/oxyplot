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

namespace PieDemo
{
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows a pie chart.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // http://www.nationsonline.org/oneworld/world_population.htm
            // http://en.wikipedia.org/wiki/Continent

            var plotModel = new PlotModel { Title = "World population by continent" };
            var pieSeries = new PieSeries();
            pieSeries.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = true });
            pieSeries.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            pieSeries.Slices.Add(new PieSlice("Asia", 4157));
            pieSeries.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            pieSeries.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });
            pieSeries.InnerDiameter = 0.2;
            pieSeries.ExplodedDistance = 0;
            pieSeries.Stroke = OxyColors.Black;
            pieSeries.StrokeThickness = 1.0;
            pieSeries.AngleSpan = 360;
            pieSeries.StartAngle = 0;
            plotModel.Series.Add(pieSeries);

            this.PieModel = plotModel;
            this.DataContext = this;
        }

        public PlotModel PieModel { get; set; }
    }
}