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
    using System.Collections.ObjectModel;

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

            Continents = new ObservableCollection<ContinentItem>();
            Continents.Add(new ContinentItem { Name = "Africa", PopulationInMillions = 1030, IsExploded = true });
            Continents.Add(new ContinentItem { Name = "Americas", PopulationInMillions = 929, IsExploded = true });
            Continents.Add(new ContinentItem { Name = "Asia", PopulationInMillions = 4157 });
            Continents.Add(new ContinentItem { Name = "Europe", PopulationInMillions = 739, IsExploded = true });
            Continents.Add(new ContinentItem { Name = "Oceania", PopulationInMillions = 35, IsExploded = true });

            this.PieModel = plotModel;
            this.DataContext = this;
        }

        public PlotModel PieModel { get; set; }

        public ObservableCollection<ContinentItem> Continents { get; private set; }
    }

    public class ContinentItem
    {
        public string Name { get; set; }

        public double PopulationInMillions { get; set; }

        public bool IsExploded { get; set; }
    }
}
