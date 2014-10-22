// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShowCases.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Showcase models
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Showcase models
    /// </summary>
    [Examples("1 ShowCases")]
    [Tags("Showcase")]
    public class ShowCases
    {
        [Example("Normal distribution")]
        public static PlotModel CreateNormalDistributionModel()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function"
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.05,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -5.25,
                Maximum = 5.25,
                MajorStep = 1,
                MinorStep = 0.25,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }

        [Example("Average (Mean) monthly temperatures in 2003")]
        public static PlotModel LineLegendPositionAtEnd()
        {
            // http://www.perceptualedge.com/example2.php
            var model = new PlotModel { Title = "Average (Mean) monthly temperatures in 2003", PlotMargins = new OxyThickness(60, 4, 60, 40), PlotAreaBorderThickness = new OxyThickness(0), IsLegendVisible = false };

            const string TrackerFormatString = "{0}: {4:0.0}ºF";
            var phoenix = new LineSeries { Title = "Phoenix", LineLegendPosition = LineLegendPosition.End, TrackerFormatString = TrackerFormatString };
            var raleigh = new LineSeries { Title = "Raleigh", LineLegendPosition = LineLegendPosition.End, TrackerFormatString = TrackerFormatString };
            var minneapolis = new LineSeries { Title = "Minneapolis", LineLegendPosition = LineLegendPosition.End, TrackerFormatString = TrackerFormatString };

            var phoenixTemps = new[] { 52.1, 55.1, 59.7, 67.7, 76.3, 84.6, 91.2, 89.1, 83.8, 72.2, 59.8, 52.5 };
            var raleighTemps = new[] { 40.5, 42.2, 49.2, 59.5, 67.4, 74.4, 77.5, 76.5, 70.6, 60.2, 50.0, 41.2 };
            var minneapolisTemps = new[] { 12.2, 16.5, 28.3, 45.1, 57.1, 66.9, 71.9, 70.2, 60.0, 50.0, 32.4, 18.6 };

            for (int i = 0; i < 12; i++)
            {
                phoenix.Points.Add(new DataPoint(i, phoenixTemps[i]));
                raleigh.Points.Add(new DataPoint(i, raleighTemps[i]));
                minneapolis.Points.Add(new DataPoint(i, minneapolisTemps[i]));
            }

            model.Series.Add(phoenix);
            model.Series.Add(raleigh);
            model.Series.Add(minneapolis);

            var categoryAxis = new CategoryAxis
            {
                AxislineStyle = LineStyle.Solid
            };
            categoryAxis.Labels.AddRange(new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" });
            model.Axes.Add(categoryAxis);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Fahrenheit", AxislineStyle = LineStyle.Solid });

            return model;
        }

        private static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance, int n = 1001)
        {
            var ls = new LineSeries
            {
                Title = string.Format("μ={0}, σ²={1}", mean, variance)
            };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + ((x1 - x0) * i / (n - 1));
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }

            return ls;
        }
    }
}