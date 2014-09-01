// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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