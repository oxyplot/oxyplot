// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSeriesExamples.cs" company="OxyPlot">
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
namespace ExampleLibrary
{
    using System;
    using OxyPlot;

    [Examples("Custom series")]
    public static class CustomSeriesExamples
    {
        [Example("ErrorSeries")]
        public static PlotModel ErrorSeries()
        {
            int n = 20;

            var model = new PlotModel("ErrorSeries");

            var s1 = new ErrorSeries { Title = "Measurements" };
            var random = new Random();
            double x = 0;
            double y = 0;
            for (int i = 0; i < n; i++)
            {
                x += 2 + (random.NextDouble() * 10);
                y += 1 + random.NextDouble();
                double xe = 1 + (random.NextDouble() * 2);
                double ye = 1 + (random.NextDouble() * 2);
                s1.Points.Add(new ErrorItem(x, y, xe, ye));
            }

            model.Series.Add(s1);
            return model;
        }

        [Example("LineSegmentSeries")]
        public static PlotModel LineSegmentSeries()
        {
            var model = new PlotModel("LineSegmentSeries");

            var lss1 = new LineSegmentSeries { Title = "The first series" };

            // First segment
            lss1.Points.Add(new DataPoint(0, 3));
            lss1.Points.Add(new DataPoint(2, 3.2));

            // Second segment
            lss1.Points.Add(new DataPoint(2, 2.7));
            lss1.Points.Add(new DataPoint(7, 2.9));

            model.Series.Add(lss1);

            var lss2 = new LineSegmentSeries { Title = "The second series" };

            // First segment
            lss2.Points.Add(new DataPoint(1, -3));
            lss2.Points.Add(new DataPoint(2, 10));

            // Second segment
            lss2.Points.Add(new DataPoint(0, 4.8));
            lss2.Points.Add(new DataPoint(7, 2.3));

            // A very short segment
            lss2.Points.Add(new DataPoint(6, 4));
            lss2.Points.Add(new DataPoint(6, 4 + 1e-8));

            model.Series.Add(lss2);

            return model;
        }

        [Example("FlagSeries")]
        public static PlotModel FlagSeries()
        {
            var model = new PlotModel("FlagSeries");

            var s1 = new FlagSeries { Title = "Incidents", Color = OxyColors.Red };
            s1.Values.Add(2);
            s1.Values.Add(3);
            s1.Values.Add(5);
            s1.Values.Add(7);
            s1.Values.Add(11);
            s1.Values.Add(13);
            s1.Values.Add(17);
            s1.Values.Add(19);

            model.Series.Add(s1);
            return model;
        }

    }
}