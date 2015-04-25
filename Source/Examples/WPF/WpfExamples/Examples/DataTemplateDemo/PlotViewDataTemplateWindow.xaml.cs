// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewDataTemplateWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for PlotViewDataTemplateWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataTemplateDemo
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;

    /// <summary>
    /// Interaction logic for PlotViewDataTemplateWindow.xaml
    /// </summary>
    [Example("Demonstrates a PlotView in a DataTemplate.")]
    public partial class PlotViewDataTemplateWindow : Window
    {
        public PlotViewDataTemplateWindow()
        {
            this.InitializeComponent();
            this.Models = CreatePlotModels();
            this.DataContext = this;
        }

        public IList<PlotModel> Models { get; private set; }

        private static Random r = new Random(13);

        private static IList<PlotModel> CreatePlotModels()
        {
            var models = new List<PlotModel>();

            for (var i = 0; i < 3; i++)
            {
                var model = new PlotModel();
                model.Title = string.Format("Plot {0}", i + 1);

                var series = new LineSeries();
                for (var j = 0; j < 10; j++)
                {
                    series.Points.Add(new DataPoint(j, r.NextDouble()));
                }

                model.Series.Add(series);

                models.Add(model);
            }

            return models;
        }
    }
}