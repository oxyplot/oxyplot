// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomSeriesExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
    using System.Collections.Generic;

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
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Rainbow(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });
            model.Series.Add(new PolarHeatMapSeries { Data = matrix, Angle0 = 30, Angle1 = 150, Magnitude0 = 0, Magnitude1 = 80, Interpolate = false });

            return model;
        }

        [Example("PolarHeatMap (interpolated)")]
        public static PlotModel PolarHeatMapInterpolated()
        {
            var model = PolarHeatMap();
            model.Title = "Polar heat map (interpolated)";
            ((PolarHeatMapSeries)model.Series[0]).Interpolate = true;
            return model;
        }

        [Example("PolarHeatMap fixed size image")]
        public static PlotModel PolarHeatMapFixed()
        {
            var model = PolarHeatMap();
            model.Title = "Polar heat map with fixed size image";
            ((PolarHeatMapSeries)model.Series[0]).ImageSize = 800;
            return model;
        }

        [Example("PolarHeatMap on linear axes")]
        public static PlotModel PolarHeatMapLinearAxes()
        {
            var model = new PlotModel { Title = "Polar heat map on linear axes" };

            var matrix = new double[2, 2];
            matrix[0, 0] = 0;
            matrix[0, 1] = 2;
            matrix[1, 0] = 1.5;
            matrix[1, 1] = 0.2;

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -100, Maximum = 100 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 100 });
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Rainbow(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });
            model.Series.Add(new PolarHeatMapSeries { Data = matrix, Angle0 = 30, Angle1 = 150, Magnitude0 = 0, Magnitude1 = 80, Interpolate = true });

            return model;
        }

        [Example("PolarHeatMap linear axes, fixed size image (256x256)")]
        public static PlotModel PolarHeatMapLinearAxesFixed256()
        {
            var model = PolarHeatMapLinearAxes();
            model.Title = "Polar heat map on linear axes & fixed size image (256x256)";
            ((PolarHeatMapSeries)model.Series[0]).ImageSize = 256;
            return model;
        }

        [Example("PolarHeatMap linear axes, fixed size image (1000x1000)")]
        public static PlotModel PolarHeatMapLinearAxesFixed1000()
        {
            var model = PolarHeatMapLinearAxes();
            model.Title = "Polar heat map on linear axes & fixed size image (1000x1000)";
            ((PolarHeatMapSeries)model.Series[0]).ImageSize = 1000;
            return model;
        }

        [Example("Design structure matrix (DSM)")]
        public static PlotModel DesignStructureMatrix()
        {
            // See also http://en.wikipedia.org/wiki/Design_structure_matrix
            var data = new double[7, 7];

            // indexing: data[column,row]
            data[1, 0] = 1;
            data[5, 0] = 1;
            data[3, 1] = 1;
            data[0, 2] = 1;
            data[6, 2] = 1;
            data[4, 3] = 1;
            data[1, 4] = 1;
            data[5, 4] = 1;
            data[2, 5] = 1;
            data[0, 6] = 1;
            data[4, 6] = 1;

            for (int i = 0; i < 7; i++)
            {
                data[i, i] = -1;
            }

            var model = new PlotModel("Design structure matrix (DSM)");
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.None, Palette = new OxyPalette(OxyColors.White, OxyColors.LightGreen), LowColor = OxyColors.Black, Minimum = 0, IsAxisVisible = false });
            model.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Top,
                Labels = new List<string>(new[] { "A", "B", "C", "D", "E", "F", "G" })
            });
            model.Axes.Add(
                new CategoryAxis
                {
                    Position = AxisPosition.Left,
                    Labels = new List<string>(new[] { "Element A", "Element B", "Element C", "Element D", "Element E", "Element F", "Element G" }),
                    StartPosition = 1,
                    EndPosition = 0
                });

            var hms = new DesignStructureMatrixSeries
            {
                Data = data,
                Interpolate = false,
                LabelFormatString = "#",
                LabelFontSize = 0.25,
                X0 = 0,
                X1 = data.GetLength(0) - 1,
                Y0 = 0,
                Y1 = data.GetLength(1) - 1,
            };

            model.Series.Add(hms);
            return model;
        }
    }

    public class DesignStructureMatrixSeries : HeatMapSeries
    {
        protected override string GetLabel(double v, int i, int j)
        {
            if (i == j)
            {
                return ((CategoryAxis)this.XAxis).Labels[i];
            }

            return base.GetLabel(v, i, j);
        }
    }
}