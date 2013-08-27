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
    using OxyPlot.Axes;
    using OxyPlot.Series;

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

        [Example("MatrixSeries - diagonal matrix")]
        public static PlotModel DiagonalMatrix()
        {
            var model = new PlotModel();

            var matrix = new double[3, 3];
            matrix[0, 0] = 1;
            matrix[1, 1] = 2;
            matrix[2, 2] = 3;

            // Reverse the vertical axis
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Series.Add(new MatrixSeries { Matrix = matrix, ShowDiagonal = true });

            return model;
        }

        [Example("PolarHeatMap")]
        public static PlotModel PolarHeatMap()
        {
            var model = new PlotModel { Title = "Polar heat map", PlotMargins = new OxyThickness(40, 80, 40, 40), PlotType = PlotType.Polar, PlotAreaBorderThickness = 0 };

            var matrix = new double[2, 2];
            matrix[0, 0] = 0;
            matrix[0, 1] = 2;
            matrix[1, 0] = 1.5;
            matrix[1, 1] = 0.2;

            model.Axes.Add(new AngleAxis { StartAngle = 0, EndAngle = 360, Minimum = 0, Maximum = 360, MajorStep = 30, MinorStep = 15 });
            model.Axes.Add(new MagnitudeAxis { Minimum = 0, Maximum = 100, MajorStep = 25, MinorStep = 5 });
            model.Axes.Add(new ColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Rainbow(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });
            model.Series.Add(new PolarHeatMapSeries { Data = matrix, Angle0 = 30, Angle1 = 150, Magnitude0 = 0, Magnitude1 = 80, Interpolate = false });

            return model;
        }

        [Example("PolarHeatMap - interpolate")]
        public static PlotModel PolarHeatMapInterpolate()
        {
            var model = new PlotModel { Title = "Polar heat map (interpolated)", PlotMargins = new OxyThickness(40, 80, 40, 40), PlotType = PlotType.Polar, PlotAreaBorderThickness = 0 };

            var matrix = new double[2, 2];
            matrix[0, 0] = 0;
            matrix[0, 1] = 2;
            matrix[1, 0] = 1.5;
            matrix[1, 1] = 0.2;

            model.Axes.Add(new AngleAxis { StartAngle = 0, EndAngle = 360, Minimum = 0, Maximum = 360, MajorStep = 30, MinorStep = 15 });
            model.Axes.Add(new MagnitudeAxis { Minimum = 0, Maximum = 100, MajorStep = 25, MinorStep = 5 });
            model.Axes.Add(new ColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Rainbow(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });
            model.Series.Add(new PolarHeatMapSeries { Data = matrix, Angle0 = 30, Angle1 = 150, Magnitude0 = 0, Magnitude1 = 80, Interpolate = true });

            return model;
        }
    }
}