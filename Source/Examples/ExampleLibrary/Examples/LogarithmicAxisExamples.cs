// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxisExamples.cs" company="OxyPlot">
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
using OxyPlot;

namespace ExampleLibrary
{
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("LogarithmicAxis")]
    public static class LogarithmicAxisExamples
    {
        [Example("Amdahl's Law")]
        public static PlotModel AmdahlsLaw()
        {
            var model = new PlotModel("Amdahl's law") { LegendTitle = "Parallel portion" };

            // http://en.wikipedia.org/wiki/Amdahl's_law
            Func<double, int, double> maxSpeedup = (p, n) => 1.0 / ((1.0 - p) + (double)p / n);
            Func<double, LineSeries> createSpeedupCurve = p =>
            {
                // todo: tracker does not work when smoothing = true (too few points interpolated on the left end of the curve)
                var ls = new LineSeries(p.ToString("P0")) { Smooth = false };
                for (int n = 1; n <= 65536; n *= 2) ls.Points.Add(new DataPoint(n, maxSpeedup(p, n)));
                return ls;
            };
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Bottom, "Number of processors") { Base = 2, MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Axes.Add(new LinearAxis(AxisPosition.Left, 0, 20, 2, 2, "Speedup") { StringFormat = "F2", MajorGridlineStyle = LineStyle.Solid, TickStyle = TickStyle.None });
            model.Series.Add(createSpeedupCurve(0.5));
            model.Series.Add(createSpeedupCurve(0.75));
            model.Series.Add(createSpeedupCurve(0.9));
            model.Series.Add(createSpeedupCurve(0.95));

            return model;
        }

        [Example("Richter magnitudes")]
        public static PlotModel RichterMagnitudes()
        {
            // http://en.wikipedia.org/wiki/Richter_magnitude_scale

            var model = new PlotModel("The Richter magnitude scale")
                            {
                                PlotMargins = new OxyThickness(80, 0, 80, 40),
                                LegendPlacement = LegendPlacement.Inside,
                                LegendPosition = LegendPosition.TopCenter,
                                LegendOrientation = LegendOrientation.Horizontal,
                                LegendSymbolLength = 24
                            };

            model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "Richter magnitude scale") { MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.None });

            var frequencyCurve = new LineSeries("Frequency")
                         {
                             Color = OxyColor.FromUInt32(0xff3c6c9e),
                             StrokeThickness = 3,
                             MarkerStroke = OxyColor.FromUInt32(0xff3c6c9e),
                             MarkerFill = OxyColors.White,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 4,
                             MarkerStrokeThickness = 3
                         };

            frequencyCurve.Points.Add(new DataPoint(1.5, 8000 * 365 * 100));
            frequencyCurve.Points.Add(new DataPoint(2.5, 1000 * 365 * 100));
            frequencyCurve.Points.Add(new DataPoint(3.5, 49000 * 100));
            frequencyCurve.Points.Add(new DataPoint(4.5, 6200 * 100));
            frequencyCurve.Points.Add(new DataPoint(5.5, 800 * 100));
            frequencyCurve.Points.Add(new DataPoint(6.5, 120 * 100));
            frequencyCurve.Points.Add(new DataPoint(7.5, 18 * 100));
            frequencyCurve.Points.Add(new DataPoint(8.5, 1 * 100));
            frequencyCurve.Points.Add(new DataPoint(9.5, 1.0 / 20 * 100));
            model.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Frequency / 100 yr") { UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.Outside });
            model.Series.Add(frequencyCurve);

            var energyCurve = new LineSeries("Energy")
            {
                Color = OxyColor.FromUInt32(0xff9e6c3c),
                StrokeThickness = 3,
                MarkerStroke = OxyColor.FromUInt32(0xff9e6c3c),
                MarkerFill = OxyColors.White,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStrokeThickness = 3
            };

            energyCurve.Points.Add(new DataPoint(1.5, 11e6));
            energyCurve.Points.Add(new DataPoint(2.5, 360e6));
            energyCurve.Points.Add(new DataPoint(3.5, 11e9));
            energyCurve.Points.Add(new DataPoint(4.5, 360e9));
            energyCurve.Points.Add(new DataPoint(5.5, 11e12));
            energyCurve.Points.Add(new DataPoint(6.5, 360e12));
            energyCurve.Points.Add(new DataPoint(7.5, 11e15));
            energyCurve.Points.Add(new DataPoint(8.5, 360e15));
            energyCurve.Points.Add(new DataPoint(9.5, 11e18));
            energyCurve.YAxisKey = "energyAxis";

            model.Axes.Add(new LogarithmicAxis(AxisPosition.Right, "Energy / J") { Key = "energyAxis", UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = TickStyle.Outside });
            model.Series.Add(energyCurve);

            return model;
        }
    }
}