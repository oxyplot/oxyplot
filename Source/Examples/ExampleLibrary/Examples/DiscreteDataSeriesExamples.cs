// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscreteDataSeriesExamples.cs" company="OxyPlot">
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
    [Examples("Discrete Data Series")]
    public static class DiscreteDataSeriesExamples
    {
        [Example("StairStepSeries")]
        public static PlotModel StairStepSeries()
        {
            var model = new PlotModel("StairStepSeries") { LegendSymbolLength = 24 };
            var s1 = new StairStepSeries("sin(x)")
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         };
            for (double x = 0; x < Math.PI * 2; x += 0.5)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }

        [Example("StairStepSeries with thin vertical lines")]
        public static PlotModel StairStepSeriesThinVertical()
        {
            var model = new PlotModel("StairStepSeries", "With thin vertical lines");
            var s1 = new StairStepSeries("sin(x)")
            {
                Color = OxyColors.SkyBlue,
                StrokeThickness = 3,
                VerticalStrokeThickness = 0.4,
                MarkerType = MarkerType.None
            };
            for (double x = 0; x < Math.PI * 2; x += 0.5)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }

        [Example("StairStepSeries with dashed vertical lines")]
        public static PlotModel StairStepSeriesDashedVertical()
        {
            var model = new PlotModel("StairStepSeries", "With dashed vertical lines");
            var s1 = new StairStepSeries("sin(x)")
            {
                Color = OxyColors.SkyBlue,
                VerticalLineStyle = LineStyle.Dash,
                MarkerType = MarkerType.None
            };
            for (double x = 0; x < Math.PI * 2; x += 0.5)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }

        [Example("StemSeries")]
        public static PlotModel StemSeries()
        {
            var model = new PlotModel("StemSeries") { LegendSymbolLength = 24 };
            var s1 = new StemSeries("sin(x)")
                         {
                             Color = OxyColors.SkyBlue,
                             MarkerType = MarkerType.Circle,
                             MarkerSize = 6,
                             MarkerStroke = OxyColors.White,
                             MarkerFill = OxyColors.SkyBlue,
                             MarkerStrokeThickness = 1.5
                         };
            for (double x = 0; x < Math.PI * 2; x += 0.1)
                s1.Points.Add(new DataPoint(x, Math.Sin(x)));
            model.Series.Add(s1);

            return model;
        }
    }
}