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

        [Example("Peaks LabelStep = 1, ContourLevelStep = PI/2")]
        public static PlotModel PeaksLabelStep1LevelStepPI2()
        {
            var model = new PlotModel { Title = "Peaks LabelStep = 1, ContourLevelStep = PI/2" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                ContourLevelStep = Math.PI / 2,
                LabelStep = 1
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks LabelStep = 2, ContourLevelStep = 0.5")]
        public static PlotModel PeaksLabelStep2()
        {
            var model = new PlotModel { Title = "Peaks LabelStep = 2, ContourLevelStep = 0.5" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                ContourLevelStep = 0.5,
                LabelStep = 2
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks LabelStep = 2, ContourLevelStep = 0.33")]
        public static PlotModel PeaksLabelStep2LevelStep033()
        {
            var model = new PlotModel { Title = "Peaks LabelStep = 2, ContourLevelStep = 0.33" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                ContourLevelStep = 0.33,
                LabelStep = 2
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks LabelStep = 3, ContourLevelStep = 1")]
        public static PlotModel PeaksLabelStep3()
        {
            var model = new PlotModel { Title = "Peaks LabelStep = 3, ContourLevelStep = 1" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                LabelStep = 3
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks MultiLabel")]
        public static PlotModel PeaksMultiLabel()
        {
            var model = new PlotModel { Title = "Peaks MultiLabel" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                MultiLabel = true
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks LabelSpacing = 400")]
        public static PlotModel PeaksLabelSpacing400()
        {
            var model = new PlotModel { Title = "Peaks LabelSpacing = 400" };
            var cs = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(-3, 3, 0.05),
                RowCoordinates = ArrayBuilder.CreateVector(-3.1, 3.1, 0.05),
                MultiLabel = true,
                LabelSpacing = 400
            };
            cs.Data = ArrayBuilder.Evaluate(peaks, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Subtitle = cs.Data.GetLength(0) + "×" + cs.Data.GetLength(1);
            model.Series.Add(cs);
            return model;
        }

        [Example("Peaks (different contour colors)")]
        [DocumentationExample("Series/ContourSeries")]
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
