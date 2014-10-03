// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContourSeriesExamples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    [Examples("ContourSeries"), Tags("Series")]
    public class ContourSeriesExamples
    {
        private static Func<double, double, double> peaks = (x, y) =>
               3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
               - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
               - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

        [Example("Peaks")]
        public static PlotModel Peaks()
        {
            var model = new PlotModel { Title = "Peaks" };
            var cs = new ContourSeries
                {
                    ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                    RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05)
                };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks (different contour colors)")]
        public static PlotModel PeaksWithColors()
        {
            var model = new PlotModel { Title = "Peaks" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                ContourColors = new[] { OxyColors.SeaGreen, OxyColors.RoyalBlue, OxyColors.IndianRed }
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks (wide array)")]
        public static PlotModel WideArrayPeaks()
        {
            var model = new PlotModel { Title = "Peaks" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -3.16262, Maximum = 3.162 });
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-1, 1, 0.05)
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

    }
}