// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotTestHarness.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Histogram;
using OxyPlot;

namespace Visiblox.Charts.Examples
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class OxyPlotTestHarness : HistogramTestHarness
    {
        private readonly OxyPlotChart _chart;

        public OxyPlotTestHarness()
        {
            // http://en.wikipedia.org/wiki/File:Cinclus_cinclus_R(ThKraft).jpg
            // Photo by Thomas Kraft
            // Creative Commons Attribution-Share Alike 2.5 Generic license.

            Image.Source = new BitmapImage(new Uri("cinclus.jpg", UriKind.Relative));
            _chart = new OxyPlotChart();
            ChartPresenter.Content = _chart;
        }

        protected override void RenderDataToChart(List<List<Histogram.DataPoint>> rgbData)
        {
            var model = new PlotModel();
            model.PlotMargins = new OxyThickness(10, 10, 10, 20);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 250, MinorStep = 50, MajorStep = 100, MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Series.Add(ListToSeries(rgbData[0], OxyColors.Red));
            model.Series.Add(ListToSeries(rgbData[1], OxyColors.Green));
            model.Series.Add(ListToSeries(rgbData[2], OxyColors.Blue));
            _chart.Chart.Model = model;
        }

        private Series ListToSeries(List<Histogram.DataPoint> data, OxyColor color)
        {
            var pts = data.Select(pt => new OxyPlot.DataPoint(pt.Location, pt.Intensity));
            return new LineSeries { ItemsSource = pts.ToList(), StrokeThickness = 1.0, Color = color };
        }
    }
}