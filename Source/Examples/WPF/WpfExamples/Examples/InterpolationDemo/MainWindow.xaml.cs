// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace InterpolationDemo
{
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Series;

    using WpfExamples;
    using System;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Shows different types of interpolation including a custom one.")]
    public partial class MainWindow
    {
        private readonly Random r = new Random(13);

        public MainWindow()
        {
            this.InitializeComponent();

            List<DataPoint> points = this.GeneratePoints(50);

            this.PlotModels = new List<PlotModel>
            {
                this.GenerateRandomPlotModel(points, "None", null),
                this.GenerateRandomPlotModel(points, "Canonical aka Cardinal",
                    new Entry("0.5 (default)", InterpolationAlgorithms.CanonicalSpline),
                    new Entry("0.1", new CanonicalSpline(0.1)),
                    new Entry("1.0", new CanonicalSpline(1.0))),
                this.GenerateRandomPlotModel(points, "Catmull–Rom",
                    new Entry("Standard", InterpolationAlgorithms.CatmullRomSpline),
                    new Entry("Uniform", InterpolationAlgorithms.UniformCatmullRomSpline),
                    new Entry("Chordal", InterpolationAlgorithms.ChordalCatmullRomSpline))
            };

            this.DataContext = this;
        }

        private class Entry
        {
            public Entry(string title, IInterpolationAlgorithm algorithm)
            {
                this.Title = title;
                this.Algorithm = algorithm;
            }

            public string Title { get; }

            public IInterpolationAlgorithm Algorithm { get; }
        }

        private List<DataPoint> GeneratePoints(int numberOfPoints)
        {
            var result = new List<DataPoint>(numberOfPoints);
            for (int i = 0; i < numberOfPoints; i++)
            {
                if (i < 5)
                {
                    result.Add(new DataPoint(i, 0.0));
                }
                else if (i < 10)
                {
                    result.Add(new DataPoint(i, 1.0));
                }
                else if (i < 12)
                {
                    result.Add(new DataPoint(i, 0.0));
                }
                else
                {
                    result.Add(new DataPoint(i, this.r.NextDouble()));
                }
            }

            return result;
        }

        public List<PlotModel> PlotModels { get; set; }

        private PlotModel GenerateRandomPlotModel(List<DataPoint> points, string title, params Entry[] entries)
        {
            var plotModel = new PlotModel
            {
                Title = title,
                TitleToolTip = title
            };

            if (entries == null)
            {
                var lineSeries = new LineSeries();
                lineSeries.Points.AddRange(points);
                plotModel.Series.Add(lineSeries);
            }
            else
            {

                foreach (var entry in entries.Reverse())
                {
                    var lineSeries = new LineSeries
                    {
                        Title = entry.Title,
                        InterpolationAlgorithm = entry.Algorithm,
                        StrokeThickness = 1.0
                    };
                    lineSeries.Points.AddRange(points);
                    plotModel.Series.Add(lineSeries);
                }
            }

            return plotModel;
        }
    }
}