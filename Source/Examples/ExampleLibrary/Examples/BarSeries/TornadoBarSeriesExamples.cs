// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TornadoBarSeriesExamples.cs" company="OxyPlot">
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
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("TornadoBarSeries")]
    public static class TornadoBarSeriesExamples
    {
        [Example("Tornado diagram 1")]
        public static PlotModel TornadoDiagram1()
        {
            // http://en.wikipedia.org/wiki/Tornado_diagram
            var model = new PlotModel("Tornado diagram 1") { LegendPlacement = LegendPlacement.Outside };

            var s1 = new BarSeries
                {
                    Title = "High",
                    IsStacked = true,
                    FillColor = OxyColor.FromRGB(216, 82, 85),
                    BaseValue = 7,
                    StrokeColor = OxyColors.Black,
                    StrokeThickness = 1
                };
            s1.Items.Add(new BarItem(1));
            s1.Items.Add(new BarItem(1));
            s1.Items.Add(new BarItem(4));
            s1.Items.Add(new BarItem(5));

            var s2 = new BarSeries
                {
                    Title = "Low",
                    IsStacked = true,
                    FillColor = OxyColor.FromRGB(84, 138, 209),
                    BaseValue = 7,
                    StrokeColor = OxyColors.Black,
                    StrokeThickness = 1
                };
            s2.Items.Add(new BarItem(-1));
            s2.Items.Add(new BarItem(-3));
            s2.Items.Add(new BarItem(-2));
            s2.Items.Add(new BarItem(-3));

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("F/X rate");
            categoryAxis.Labels.Add("Inflation");
            categoryAxis.Labels.Add("Price");
            categoryAxis.Labels.Add("Conversion");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { ExtraGridlines = new[] { 7.0 } };
            model.Series.Add(s1);
            model.Series.Add(s2);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        [Example("Tornado diagram 2")]
        public static PlotModel TornadoDiagram2()
        {
            var model = new PlotModel("Tornado diagram 2") { LegendPlacement = LegendPlacement.Outside };

            var s1 = new TornadoBarSeries { Title = "TornadoBarSeries", BaseValue = 7 };
            s1.Items.Add(new TornadoBarItem { Minimum = 6, Maximum = 8 });
            s1.Items.Add(new TornadoBarItem { Minimum = 4, Maximum = 8 });
            s1.Items.Add(new TornadoBarItem { Minimum = 5, Maximum = 11 });
            s1.Items.Add(new TornadoBarItem { Minimum = 4, Maximum = 12 });

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            categoryAxis.Labels.Add("F/X rate");
            categoryAxis.Labels.Add("Inflation");
            categoryAxis.Labels.Add("Price");
            categoryAxis.Labels.Add("Conversion");
            var valueAxis = new LinearAxis(AxisPosition.Bottom) { ExtraGridlines = new[] { 7.0 }, MinimumPadding = 0.1, MaximumPadding = 0.1 };
            model.Series.Add(s1);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }
    }
}