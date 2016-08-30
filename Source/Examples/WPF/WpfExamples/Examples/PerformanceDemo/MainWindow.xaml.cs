// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace PerformanceDemo
{
    using System.Collections.Generic;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows OxyPlot performance when using a large number of plots in a list.")]
    public partial class MainWindow : Window
    {
        Random r = new Random(13);

        public MainWindow()
        {
            this.InitializeComponent();

            PlotModels = new List<PlotModel>();
            for (int i = 0; i < 250; i++)
            {
                PlotModels.Add(GenerateRandomPlotModel(string.Format("Random plot '{0}'", i + 1)));
            }

            this.DataContext = this;
        }

        public List<PlotModel> PlotModels { get; set; }

        private PlotModel GenerateRandomPlotModel(string title, int numberOfPoints = 50)
        {
            var plotModel = new PlotModel
            {
                Title = title,
                TitleToolTip = title
            };
            var lineSeries = new LineSeries();

            for (int i = 0; i < numberOfPoints; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, this.r.NextDouble()));
            }

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }
    }
}