// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPlotTestHarness.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using Histogram;
using OxyPlot;

namespace Visiblox.Charts.Examples
{
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
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 250, 50, 100) { MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 0) { MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Series.Add(ListToSeries(rgbData[0], OxyColors.Red));
            model.Series.Add(ListToSeries(rgbData[1], OxyColors.Green));
            model.Series.Add(ListToSeries(rgbData[2], OxyColors.Blue));
            _chart.Chart.Model = model;
        }

        private ISeries ListToSeries(List<Histogram.DataPoint> data, OxyColor color)
        {
            var pts = data.Select(pt => new OxyPlot.DataPoint(pt.Location, pt.Intensity));
            return new OxyPlot.LineSeries { Points = pts.ToList(), StrokeThickness = 1.0, Color = color };
        }
    }
}