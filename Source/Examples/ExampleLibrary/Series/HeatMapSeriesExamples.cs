// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Creates a simple example heat map from a 2×3 matrix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("HeatMapSeries"), Tags("Series")]
    public class HeatMapSeriesExamples
    {
        [Example("Peaks")]
        [DocumentationExample("Series/HeatMapSeries")]
        public static PlotModel Peaks()
        {
            return CreatePeaks();
        }

        public static PlotModel CreatePeaks(OxyPalette palette = null, bool includeContours = true, int n = 100)
        {
            double x0 = -3.1;
            double x1 = 3.1;
            double y0 = -3;
            double y1 = 3;
            Func<double, double, double> peaks = (x, y) => 3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1)) - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y) - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);
            var xvalues = ArrayBuilder.CreateVector(x0, x1, n);
            var yvalues = ArrayBuilder.CreateVector(y0, y1, n);
            var peaksData = ArrayBuilder.Evaluate(peaks, xvalues, yvalues);

            var model = new PlotModel { Title = "Peaks" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = palette ?? OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData };
            model.Series.Add(hms);
            if (includeContours)
            {
                var cs = new ContourSeries
                {
                    Color = OxyColors.Black,
                    FontSize = 0,
                    ContourLevelStep = 1,
                    LabelBackground = OxyColors.Undefined,
                    ColumnCoordinates = yvalues,
                    RowCoordinates = xvalues,
                    Data = peaksData
                };
                model.Series.Add(cs);
            }

            return model;
        }

        [Example("2×3, interpolated")]
        public static PlotModel Interpolated()
        {
            return CreateExample("Interpolated", true);
        }

        [Example("2×3, interpolated, cartesian axes")]
        public static PlotModel InterpolatedCartesian()
        {
            var model = CreateExample("Interpolated, cartesian axes", true);
            model.PlotType = PlotType.Cartesian;
            return model;
        }

        [Example("2×3, interpolated with two NaN values")]
        public static PlotModel InterpolatedWithNanValue()
        {
            var model = CreateExample("Interpolated including two NaN values", true);
            var hms = (HeatMapSeries)model.Series[0];
            hms.Data[0, 1] = double.NaN;
            hms.Data[1, 0] = double.NaN;
            return model;
        }

        [Example("2×3, interpolated with two NaN values, flat data")]
        public static PlotModel InterpolatedWithNanValueFlat()
        {
            var model = CreateExample("Interpolated including two NaN values, otherwise 4.71", true);
            var hms = (HeatMapSeries)model.Series[0];

            double datum = 4.71d;
            hms.Data[0, 0] = datum;
            hms.Data[0, 1] = datum;
            hms.Data[0, 2] = datum;
            hms.Data[1, 0] = datum;
            hms.Data[1, 1] = datum;
            hms.Data[1, 2] = datum;

            hms.Data[0, 1] = double.NaN;
            hms.Data[1, 0] = double.NaN;
            return model;
        }

        [Example("2×3, not interpolated")]
        public static PlotModel NotInterpolated()
        {
            return CreateExample("Not interpolated values", false);
        }

        [Example("2×3, not interpolated with two NaN values")]
        public static PlotModel NotInterpolatedWithNanValue()
        {
            var model = CreateExample("Not interpolated values including two NaN values", false);
            var ca = (LinearColorAxis)model.Axes[0];
            ca.InvalidNumberColor = OxyColors.Transparent;
            var hms = (HeatMapSeries)model.Series[0];
            hms.Data[0, 1] = double.NaN;
            hms.Data[1, 0] = double.NaN;
            return model;
        }

        [Example("2×3, not interpolated with two NaN values, flat data")]
        public static PlotModel NotInterpolatedWithNanValueFlat()
        {
            var model = CreateExample("Not interpolated values including two NaN values, otherwise 4.71", false);
            var ca = (LinearColorAxis)model.Axes[0];
            ca.InvalidNumberColor = OxyColors.Transparent;
            var hms = (HeatMapSeries)model.Series[0];

            double datum = 4.71d;
            hms.Data[0, 0] = datum;
            hms.Data[0, 1] = datum;
            hms.Data[0, 2] = datum;
            hms.Data[1, 0] = datum;
            hms.Data[1, 1] = datum;
            hms.Data[1, 2] = datum;

            hms.Data[0, 1] = double.NaN;
            hms.Data[1, 0] = double.NaN;
            return model;
        }

        [Example("2×3, reversed x-axis")]
        public static PlotModel NotInterpolatedReversedX()
        {
            var model = CreateExample("Reversed x-axis", false);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            return model;
        }

        [Example("2×3, X0>X1")]
        public static PlotModel X0GreaterThanX1()
        {
            var model = CreateExample("X0>X1", false);
            var hms = (HeatMapSeries)model.Series[0];
            var tmp = hms.X0;
            hms.X0 = hms.X1;
            hms.X1 = tmp;
            return model;
        }

        [Example("2×3, reversed x-axis, X0>X1")]
        public static PlotModel ReversedX_X0GreaterThanX1()
        {
            var model = CreateExample("Reversed x-axis, X0>X1", false);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            var hms = (HeatMapSeries)model.Series[0];
            var tmp = hms.X0;
            hms.X0 = hms.X1;
            hms.X1 = tmp;
            return model;
        }

        [Example("2×3, reversed y-axis")]
        public static PlotModel NotInterpolatedReversedY()
        {
            var model = CreateExample("Reversed y-axis", false);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            return model;
        }

        [Example("2×3, Y0>Y1")]
        public static PlotModel Y0GreaterThanY1()
        {
            var model = CreateExample("Y0>Y1", false);
            var hms = (HeatMapSeries)model.Series[0];
            var tmp = hms.Y0;
            hms.Y0 = hms.Y1;
            hms.Y1 = tmp;
            return model;
        }

        [Example("2×3, reversed y-axis, Y0>Y1")]
        public static PlotModel ReversedY_Y0GreaterThanY1()
        {
            var model = CreateExample("Reversed y-axis, Y0>Y1", false);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            var hms = (HeatMapSeries)model.Series[0];
            var tmp = hms.Y0;
            hms.Y0 = hms.Y1;
            hms.Y1 = tmp;
            return model;
        }

        [Example("2x3, reversed x- and y-axis")]
        public static PlotModel NotInterpolatedReversedXY()
        {
            var model = CreateExample("Reversed x- and y-axis", false);
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, StartPosition = 1, EndPosition = 0 });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, StartPosition = 1, EndPosition = 0 });
            return model;
        }

        [Example("3×3, diagonal (center defined)")]
        public static PlotModel Diagonal()
        {
            var data = new double[3, 3];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;

            var model = new PlotModel { Title = "Diagonal (center defined)" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            // adding half cellwidth/cellheight to bounding box coordinates
            var hms = new HeatMapSeries { CoordinateDefinition = HeatMapCoordinateDefinition.Center, X0 = 0.5, X1 = 2.5, Y0 = 2.5, Y1 = 0.5, Data = data, Interpolate = false };
            model.Series.Add(hms);
            return model;
        }

        [Example("3×3, diagonal (edge defined)")]
        public static PlotModel Diagonal2()
        {
            var data = new double[3, 3];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;

            var model = new PlotModel { Title = "Diagonal (edge defined)" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            // adding half cellwidth/cellheight to bounding box coordinates
            var hms = new HeatMapSeries { CoordinateDefinition = HeatMapCoordinateDefinition.Edge, X0 = 0, X1 = 3, Y0 = 3, Y1 = 0, Data = data, Interpolate = false };
            model.Series.Add(hms);
            return model;
        }

        [Example("6×6, diagonal")]
        public static PlotModel Diagonal_6X6()
        {
            var data = new double[6, 6];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;
            data[3, 3] = 1;
            data[4, 4] = 1;
            data[5, 5] = 1;

            var model = new PlotModel { Title = "Diagonal 6×6" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            // note: the coordinates are specifying the centers of the edge cells
            var hms = new HeatMapSeries { X0 = 0, X1 = 5, Y0 = 5, Y1 = 0, Data = data, Interpolate = false };
            model.Series.Add(hms);
            return model;
        }

        [Example("Confusion matrix")]
        public static PlotModel ConfusionMatrix()
        {
            // Example provided by Pau Climent Pérez
            // See also http://en.wikipedia.org/wiki/Confusion_matrix
            var data = new double[3, 3];

            data[0, 0] = 1;
            data[1, 1] = 0.8;
            data[1, 2] = 0.2;
            data[2, 2] = 1;

            // I guess this is where the confusion comes from?
            data = data.Transpose();

            string[] cat1 = { "class A", "class B", "class C" };

            var model = new PlotModel { Title = "Confusion Matrix" };

            var palette = OxyPalette.Interpolate(50, OxyColors.White, OxyColors.Black);

            var lca = new LinearColorAxis { Position = AxisPosition.Right, Palette = palette, HighColor = OxyColors.White, LowColor = OxyColors.White };
            model.Axes.Add(lca);

            var axis1 = new CategoryAxis { Position = AxisPosition.Top, Title = "Actual class" };
            axis1.Labels.AddRange(cat1);
            model.Axes.Add(axis1);

            // We invert this axis, so that they look "symmetrical"
            var axis2 = new CategoryAxis { Position = AxisPosition.Left, Title = "Predicted class" };
            axis2.Labels.AddRange(cat1);
            axis2.Angle = -90;
            axis2.StartPosition = 1;
            axis2.EndPosition = 0;

            model.Axes.Add(axis2);

            var hms = new HeatMapSeries
            {
                Data = data,
                Interpolate = false,
                LabelFontSize = 0.25,
                X0 = 0,
                X1 = data.GetLength(1) - 1,
                Y0 = 0,
                Y1 = data.GetLength(0) - 1,
            };

            model.Series.Add(hms);
            return model;
        }

        [Example("Logarithmic X, interpolated")]
        public static PlotModel LogXInterpolated()
        {
            var data = new double[11, 21];

            double k = Math.Pow(2, 0.1);

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    data[i, j] = Math.Pow(k, (double)i) * (double)j / 40.0;
                }
            }

            var model = new PlotModel { Title = "Logarithmic X, interpolated" };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Gray(500), HighColor = OxyColors.White, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries { X0 = 1.0, X1 = 2.0, Y0 = 0, Y1 = 20, Data = data, Interpolate = true };

            model.Series.Add(hms);
            return model;
        }

        [Example("Logarithmic X, discrete rectangles")]
        public static PlotModel LogXNotInterpolated()
        {
            var data = new double[11, 21];

            double k = Math.Pow(2, 0.1);

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    data[i, j] = Math.Pow(k, (double)i) * (double)j / 40.0;
                }
            }

            var model = new PlotModel { Title = "Logarithmic X, discrete rectangles" };
            model.Axes.Add(new LogarithmicAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Gray(500), HighColor = OxyColors.White, LowColor = OxyColors.Black });

            var hms = new HeatMapSeries { X0 = 1.0, X1 = 2.0, Y0 = 0, Y1 = 20, Data = data, Interpolate = false, RenderMethod = HeatMapRenderMethod.Rectangles, LabelFontSize = 0.4 };

            model.Series.Add(hms);
            return model;
        }

        [Example("6×4, not transposed")]
        public static PlotModel Normal_6X4()
        {
            return Create6X4("Normal 6×4 Heatmap");
        }

        private static PlotModel Create6X4(string title)
        {
            var data = new double[6, 4];

            for (int i = 1; i <= 6; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    data[i - 1, j - 1] = i * j;
                }
            }

            var model = new PlotModel { Title = title, Subtitle = "Note the positions of the axes" };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.White, LowColor = OxyColors.Black });
            model.Series.Add(new HeatMapSeries { X0 = 1, X1 = 6, Y0 = 1, Y1 = 4, Data = data, Interpolate = true, LabelFontSize = 0.2 });
            return model;
        }

        /// <summary>
        /// Creates a simple example heat map from a 2×3 matrix.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="interpolate">Interpolate the HeatMapSeries if set to <c>true</c>.</param>
        /// <returns>A <see cref="PlotModel" />.</returns>
        private static PlotModel CreateExample(string title, bool interpolate)
        {
            var data = new double[2, 3];
            data[0, 0] = 0;
            data[0, 1] = 0.2;
            data[0, 2] = 0.4;
            data[1, 0] = 0.1;
            data[1, 1] = 0.3;
            data[1, 2] = 0.2;

            var model = new PlotModel { Title = "HeatMapSeries", Subtitle = title };
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });

            // adding half cellwidth/cellheight to bounding box coordinates
            var hms = new HeatMapSeries
            {
                CoordinateDefinition = HeatMapCoordinateDefinition.Center,
                X0 = 0.5,
                X1 = 1.5,
                Y0 = 0.5,
                Y1 = 2.5,
                Data = data,
                Interpolate = interpolate,
                LabelFontSize = 0.2,
            };

            model.Series.Add(hms);
            return model;
        }
    }

    internal static class ArrayExtensions
    {
        public static double[,] Transpose(this double[,] input)
        {
            int m = input.GetLength(0);
            int n = input.GetLength(1);
            var output = new double[n, m];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    output[j, i] = input[i, j];
                }
            }

            return output;
        }
    }
}
